using System.Collections;
using System.Collections.Generic;
using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using NewHabr.Business.Services;
using NewHabr.DAL.Repository;
using NewHabr.Domain;
using NewHabr.Domain.ConfigurationModels;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;
using NewHabr.Domain.ServiceModels;

namespace UnitTests.Services;

public class UserServiceTest : ServiceTestsBase
{
    private readonly IUserService _sut;

    private readonly User _user;
    private readonly AppSettings _appSettings;


    public UserServiceTest()
    {
        _user = new User { Id = Guid.NewGuid() };
        _userRepositoryMock
            .Setup(ur => ur.GetByIdAsync(_user.Id, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_user);

        var opt = Options.Create(new AppSettings { UserBanExpiresInDays = 14 });
        _appSettings = opt.Value;
        _sut = new UserService(opt, _mapper, _repositoryManagerMock.Object, null, null, null);
    }

    [Fact]
    public async Task SetBanOnUser_BanPropertiesSetOnUser()
    {
        // arrange
        var userBanProperties = _fixture
            .Create<UserBanDto>();

        // act
        await _sut.SetBanOnUserAsync(_user.Id, userBanProperties, CancellationToken.None);

        // assert
        Assert.True(_user.Banned);
        Assert.Equal(userBanProperties.BanReason, _user.BanReason);
        Assert.NotNull(_user.BannedAt);
        Assert.InRange((DateTimeOffset)_user.BannedAt, DateTimeOffset.UtcNow.AddMinutes(-1), DateTimeOffset.UtcNow.AddMinutes(1));
        Assert.InRange(
            (DateTimeOffset)_user.BanExpiratonDate,
            DateTimeOffset.UtcNow.AddMinutes(-1).AddDays(_appSettings.UserBanExpiresInDays),
            DateTimeOffset.UtcNow.AddMinutes(1).AddDays(_appSettings.UserBanExpiresInDays));
        _repositoryManagerMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SetBanOnUser_UserNotExist_ThrowUserNotFoundException()
    {
        // arrange
        var userBanProperties = _fixture
            .Create<UserBanDto>();
        _userRepositoryMock
            .Reset();

        // act
        async Task Act() => await _sut.SetBanOnUserAsync(_user.Id, userBanProperties, CancellationToken.None);

        // assert
        await Assert.ThrowsAsync<UserNotFoundException>(Act);
        _repositoryManagerMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUserProfileAsync_UserNotExist_ThrowUserNotFoundException()
    {
        // arrange
        var dto = _fixture
            .Create<UserForManipulationDto>();
        _userRepositoryMock
            .Reset();

        // act
        async Task Act() => await _sut.UpdateUserProfileAsync(_user.Id, dto, CancellationToken.None);

        // assert
        await Assert.ThrowsAsync<UserNotFoundException>(Act);
        _repositoryManagerMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUserProfileAsync_UserExist_SaveAsyncCalledOnce()
    {
        // arrange
        var bDay = _fixture
            .Create<DateTimeOffset>()
            .UtcDateTime;
        var dto = _fixture
            .Build<UserForManipulationDto>()
            .With(x => x.BirthDay, new DateTimeOffset(bDay).ToUnixTimeMilliseconds())
            .Create();

        // act
        await _sut.UpdateUserProfileAsync(_user.Id, dto, CancellationToken.None);

        // assert
        Assert.Equal(dto.FirstName, _user.FirstName);
        Assert.Equal(dto.LastName, _user.LastName);
        Assert.Equal(dto.Patronymic, _user.Patronymic);
        Assert.Equal(dto.Description, _user.Description);
        Assert.Equal(bDay, _user.BirthDay!.Value, TimeSpan.FromMinutes(10));

        _repositoryManagerMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(253402300800000)]
    [InlineData(-62135596800001)]
    public async Task UpdateUserProfileAsync_BDayNotInRange_ThrowArgumentException(long date)
    {
        // arrange
        // Valid values are between -62135596800000 and 253402300799999, inclusive. (Parameter 'milliseconds')
        var dto = new UserForManipulationDto { BirthDay = date };

        // act
        async Task Act() => await _sut.UpdateUserProfileAsync(_user.Id, dto, CancellationToken.None);

        // assert
        await Assert.ThrowsAsync<ArgumentException>(Act);
        _repositoryManagerMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetUserArticlesAsync_QueryingOthersArticles_CallGetByAuthorIdAsyncMethod()
    {
        // arrange
        var articleModels = _fixture
            .Build<ArticleModel>()
            .Without(a => a.Comments)
            .Without(a => a.Categories)
            .Without(a => a.Tags)
            .CreateMany()
            .ToList();

        _articleRepositoryMock
            .Setup(ar => ar.GetByAuthorIdAsync(_user.Id, It.IsAny<Guid>(), false, It.IsAny<ArticleQueryParameters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedList<ArticleModel>(articleModels, 10, 1, 100));

        // act
        var articles = await _sut.GetUserArticlesAsync(_user.Id, Guid.NewGuid(), It.IsAny<ArticleQueryParametersDto>(), CancellationToken.None);

        // assert
        Assert.NotNull(articles);
        Assert.NotEmpty(articles.Articles);
        Assert.IsAssignableFrom<ArticlesGetResponse>(articles);
        Assert.IsAssignableFrom<ICollection<ArticleDto>>(articles.Articles);
        _articleRepositoryMock.Verify(
            ar => ar.GetByAuthorIdAsync(_user.Id, It.IsAny<Guid>(), false, It.IsAny<ArticleQueryParameters>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _articleRepositoryMock
            .Verify(
                ar => ar.GetAllByAuthorIdAsync(_user.Id, false, It.IsAny<ArticleQueryParameters>(), It.IsAny<CancellationToken>()),
                Times.Never);
        _repositoryManagerMock.Verify(rm => rm.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetUserArticlesAsync_QueryingOwnArticles_CallGetAllByAuthorIdAsyncMethod()
    {
        // arrange
        var articleModels = _fixture
            .Build<ArticleModel>()
            .Without(a => a.Comments)
            .Without(a => a.Categories)
            .Without(a => a.Tags)
            .CreateMany()
            .ToList();

        _articleRepositoryMock
            .Setup(ar => ar.GetAllByAuthorIdAsync(_user.Id, false, It.IsAny<ArticleQueryParameters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedList<ArticleModel>(articleModels, 10, 1, 100));

        // act
        var articles = await _sut.GetUserArticlesAsync(_user.Id, _user.Id, It.IsAny<ArticleQueryParametersDto>(), CancellationToken.None);

        // assert
        Assert.NotNull(articles);
        Assert.NotEmpty(articles.Articles);
        Assert.IsAssignableFrom<ArticlesGetResponse>(articles);
        Assert.IsAssignableFrom<ICollection<ArticleDto>>(articles.Articles);
        _articleRepositoryMock
            .Verify(
                ar => ar.GetAllByAuthorIdAsync(_user.Id, false, It.IsAny<ArticleQueryParameters>(), It.IsAny<CancellationToken>()),
                Times.Once);
        _articleRepositoryMock
            .Verify(ar => ar.GetByAuthorIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), false, It.IsAny<ArticleQueryParameters>(), It.IsAny<CancellationToken>()),
                    Times.Never);
        _repositoryManagerMock.Verify(rm => rm.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetUserArticlesAsync_UserNotExists_ThrowUserNotFoundException()
    {
        // arrange
        _userRepositoryMock.Reset();

        // act
        async Task Act() => await _sut.GetUserArticlesAsync(_user.Id, Guid.Empty, null, CancellationToken.None);

        // assert
        await Assert.ThrowsAsync<UserNotFoundException>(Act);

        _articleRepositoryMock.Verify(
            ar => ar.GetByAuthorIdAsync(_user.Id, It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<ArticleQueryParameters>(), It.IsAny<CancellationToken>()), Times.Never);

        _repositoryManagerMock.Verify(
            rm => rm.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetUserNotificationsAsync_ReturnsCollectionNotifications()
    {
        // arrange
        var notifications = _fixture
            .Build<Notification>()
            .Without(n => n.Users)
            .CreateMany()
            .ToList();
        _notificationRepositoryMock
            .Setup(nr => nr.GetUserNotificationsAsync(_user.Id, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(notifications);

        // act
        var notificationDto = await _sut.GetUserNotificationsAsync(_user.Id, false, CancellationToken.None);

        // assert
        Assert.NotNull(notificationDto);
        Assert.NotEmpty(notificationDto);
    }

    [Fact]
    public async Task GetUserNotificationsAsync_UserNotExists_ThrowUserNotFoundException()
    {
        // arrange
        _userRepositoryMock.Reset();

        // act
        var Act = async () => await _sut.GetUserNotificationsAsync(_user.Id, false, CancellationToken.None);

        // assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => Act());
    }
}
