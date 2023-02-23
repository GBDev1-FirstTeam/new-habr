using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using NewHabr.Business.AutoMapperProfiles;
using NewHabr.Business.Services;
using NewHabr.Domain;
using NewHabr.Domain.ConfigurationModels;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Repositories;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;
using NewHabr.Domain.ServiceModels;

namespace UnitTests;

public class UserServiceTest
{
    private readonly IUserService _userService;
    private readonly Mock<IRepositoryManager> _repositoryManagerMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<INotificationRepository> _notificationRepositoryMock;
    private readonly Mock<IArticleRepository> _articleRepositoryMock;
    private readonly IMapper _mapper;
    private readonly User _user;

    public UserServiceTest()
    {
        _user = new User { Id = Guid.NewGuid() };
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(typeof(MapperProfile))));
        _userRepositoryMock = new Mock<IUserRepository>();
        _userRepositoryMock
            .Setup(ur => ur.GetByIdAsync(_user.Id, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_user);

        _articleRepositoryMock = new Mock<IArticleRepository>();

        _notificationRepositoryMock = new Mock<INotificationRepository>();

        _repositoryManagerMock = new Mock<IRepositoryManager>();
        _repositoryManagerMock
            .Setup(repman => repman.UserRepository).Returns(_userRepositoryMock.Object);
        _repositoryManagerMock
            .Setup(repman => repman.ArticleRepository).Returns(_articleRepositoryMock.Object);
        _repositoryManagerMock
            .Setup(repman => repman.NotificationRepository).Returns(_notificationRepositoryMock.Object);

        var opt = Options.Create(new AppSettings { UserBanExpiresInDays = 14 });
        _userService = new UserService(opt, _mapper, _repositoryManagerMock.Object, null, null);
    }

    [Fact]
    public async Task SetBanOnUser_ValidState_SaveAsyncCalledOnce()
    {
        // arrange

        // act
        await _userService.SetBanOnUserAsync(_user.Id, new UserBanDto { BanReason = "unit testing" }, CancellationToken.None);

        // assert
        Assert.Equal("unit testing", _user.BanReason);

        Assert.NotNull(_user.BannedAt);
        Assert.InRange((DateTimeOffset)_user.BannedAt!, DateTimeOffset.UtcNow.AddMinutes(-1), DateTimeOffset.UtcNow.AddMinutes(1));

        Assert.NotNull(_user.BanExpiratonDate);

        _repositoryManagerMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SetBanOnUser_UserNotExist_ThrowUserNotFoundException()
    {
        // arrange
        var banDto = new UserBanDto { BanReason = "unit testing" };
        _userRepositoryMock.Reset();

        // act
        async Task Act() => await _userService.SetBanOnUserAsync(_user.Id, banDto, CancellationToken.None);

        // assert
        await Assert.ThrowsAsync<UserNotFoundException>(Act);
        _repositoryManagerMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUserProfileAsync_UserNotExist_ThrowUserNotFoundException()
    {
        // arrange
        var dto = new UserForManipulationDto();
        _userRepositoryMock.Reset();

        // act
        async Task Act() => await _userService.UpdateUserProfileAsync(_user.Id, dto, CancellationToken.None);

        // assert
        await Assert.ThrowsAsync<UserNotFoundException>(Act);
        _repositoryManagerMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUserProfileAsync_UserExist_SaveAsyncCalledOnce()
    {
        // arrange
        long bday = new DateTimeOffset(2018, 02, 25, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();
        UserForManipulationDto dto = new UserForManipulationDto
        {
            BirthDay = bday,
            Description = "desc",
            FirstName = "fName",
            LastName = "lName",
            Patronymic = "pName"
        };

        // act
        await _userService.UpdateUserProfileAsync(_user.Id, dto, CancellationToken.None);

        // assert
        Assert.Equal("fName", _user.FirstName);
        Assert.Equal("lName", _user.LastName);
        Assert.Equal("pName", _user.Patronymic);
        Assert.Equal("desc", _user.Description);
        Assert.Equal(new DateTime(2018, 02, 25), _user.BirthDay);

        _repositoryManagerMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserProfileAsync_BDayNotInRange_ThrowArgumentException()
    {
        // arrange
        UserForManipulationDto dto = new UserForManipulationDto
        {
            // Valid values are between -62135596800000 and 253402300799999, inclusive. (Parameter 'milliseconds')
            BirthDay = long.MaxValue
        };

        // act
        async Task Act() => await _userService.UpdateUserProfileAsync(_user.Id, dto, CancellationToken.None);

        // assert
        await Assert.ThrowsAsync<ArgumentException>(Act);
        _repositoryManagerMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetUserArticlesAsync_UserExists_ReturnArticles()
    {
        // arrange
        DateTimeOffset dto = new DateTimeOffset(2018, 02, 25, 0, 0, 0, TimeSpan.Zero);

        _articleRepositoryMock
            .Setup(ar => ar.GetByAuthorIdAsync(_user.Id, It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<ArticleQueryParameters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedList<ArticleModel>(new List<ArticleModel>
            {
                new ArticleModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Title1",
                    CreatedAt = dto,
                    ModifiedAt = dto,
                    PublishedAt = dto
                },
                new ArticleModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Title2",
                    CreatedAt = dto,
                    ModifiedAt = dto,
                    PublishedAt = dto
                }
            }, 10, 1, 100));

        // act
        var articles = await _userService.GetUserArticlesAsync(_user.Id, Guid.Empty, null, CancellationToken.None);

        // assert
        Assert.NotNull(articles);
        Assert.NotEmpty(articles.Articles);
        Assert.IsAssignableFrom<ICollection<ArticleDto>>(articles);
        Assert.All(articles.Articles, item =>
        {
            Assert.Equal(item.ModifiedAt, dto.ToUnixTimeMilliseconds());
            Assert.Equal(item.CreatedAt, dto.ToUnixTimeMilliseconds());
            Assert.Equal(item.PublishedAt, dto.ToUnixTimeMilliseconds());
        });
        _repositoryManagerMock.Verify(rm => rm.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetUserArticlesAsync_UserNotExists_ThrowUserNotFoundException()
    {
        // arrange
        _userRepositoryMock.Reset();

        // act
        async Task Act() => await _userService.GetUserArticlesAsync(_user.Id, Guid.Empty, null, CancellationToken.None);

        // assert
        await Assert.ThrowsAsync<UserNotFoundException>(Act);

        _articleRepositoryMock.Verify(
            ar => ar.GetByAuthorIdAsync(_user.Id, It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<ArticleQueryParameters>(), It.IsAny<CancellationToken>()), Times.Never);

        _repositoryManagerMock.Verify(
            rm => rm.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetUserNotificationsAsync_as_ReturnsCollectionNotifications()
    {
        // arrange
        _notificationRepositoryMock
            .Setup(nr => nr.GetUserNotificationsAsync(_user.Id, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Notification>
            {
                new Notification()
            });

        // act
        var notificationDto = await _userService.GetUserNotificationsAsync(_user.Id, false, CancellationToken.None);

        // assert
        Assert.NotNull(notificationDto);
    }
}
