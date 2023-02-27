using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using NewHabr.Business.AutoMapperProfiles;
using NewHabr.Business.Services;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace UnitTests;

public class CommentServiceTests
{
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly Mock<IRepositoryManager> _repositoryManagerMock;
    private readonly Mock<ICommentRepository> _commentRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IArticleRepository> _articleRepositoryMock;
    private Mock<UserManager<User>> _userManagerMock;
    private readonly IMapper _mapper;

    private readonly User _user;
    private readonly CommentService _sut;


    public CommentServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(typeof(MapperProfile))));
        _commentRepositoryMock = new();
        _articleRepositoryMock = new();
        _userRepositoryMock = new();
        _repositoryManagerMock = new();
        _notificationServiceMock = new();

        _user = new User { Id = Guid.NewGuid() };
        _userRepositoryMock
            .Setup(ur => ur.GetByIdAsync(_user.Id, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_user);

        _articleRepositoryMock
            .Setup(ar => ar.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Article { Id = Guid.NewGuid(), Title = "Mocked Article Title", Content = "Mocked Article Content" });

        _repositoryManagerMock
            .Setup(repman => repman.CommentRepository)
            .Returns(_commentRepositoryMock.Object);
        _repositoryManagerMock
            .Setup(repman => repman.UserRepository)
            .Returns(_userRepositoryMock.Object);
        _repositoryManagerMock
            .Setup(repman => repman.ArticleRepository)
            .Returns(_articleRepositoryMock.Object);

        ConfigureUserManagerMocking();

        _sut = new CommentService(_repositoryManagerMock.Object, _mapper, _notificationServiceMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_NoOneMentioned_CreatesCommentWithOutNotifications()
    {
        // arrange
        var comment = new CommentCreateRequest
        {
            ArticleId = Guid.NewGuid(),
            Text = "some text"
        };

        // act
        await _sut.CreateAsync(_user.Id, comment, default);

        // assert
        _repositoryManagerMock.Verify(rm => rm.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ModeratorMentioned_3InRole_CreatesCommentAndNotifications()
    {
        // arrange
        var comment = new CommentCreateRequest
        {
            ArticleId = Guid.NewGuid(),
            Text = "@moderator some text"
        };

        _userManagerMock
            .Setup(userManager => userManager.GetUsersInRoleAsync(UserRoles.Moderator.ToString()))
            .ReturnsAsync(new List<User>
            {
                        new User{Id = Guid.NewGuid()},
                        new User{Id = Guid.NewGuid()},
                        new User{Id = Guid.NewGuid()}
            });

        // act
        await _sut.CreateAsync(_user.Id, comment, default);

        // assert
        _repositoryManagerMock
            .Verify(rm => rm.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _notificationServiceMock
            .Verify(ns => ns.CreateAsync(It.IsAny<NotificationCreateRequest>(), It.IsAny<ICollection<User>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ModeratorMentioned_NoneInRole_CreatesCommentWithOutNotifications()
    {
        // arrange
        var comment = new CommentCreateRequest
        {
            ArticleId = Guid.NewGuid(),
            Text = "@moderator some text"
        };

        _userManagerMock
            .Setup(userManager => userManager.GetUsersInRoleAsync(UserRoles.Moderator.ToString()))
            .ReturnsAsync(new List<User>());

        // act
        await _sut.CreateAsync(_user.Id, comment, default);

        // assert
        _repositoryManagerMock
            .Verify(rm => rm.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _notificationServiceMock
            .Verify(ns => ns.CreateAsync(It.IsAny<NotificationCreateRequest>(), It.IsAny<ICollection<User>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_FakeUserMentioned_CreatesCommentWithOutNotifications()
    {
        // arrange
        var comment = new CommentCreateRequest
        {
            ArticleId = Guid.NewGuid(),
            Text = "@JohnDowe some text"
        };

        _userRepositoryMock
            .Setup(ur => ur.GetUsersByLoginAsync(It.IsAny<ICollection<string>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User>());

        // act
        await _sut.CreateAsync(_user.Id, comment, default);

        // assert
        _repositoryManagerMock
            .Verify(rm => rm.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _notificationServiceMock
            .Verify(ns => ns.CreateAsync(It.IsAny<NotificationCreateRequest>(), It.IsAny<ICollection<User>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_RealUserMentioned_CreatesCommentWithNotifications()
    {
        // arrange
        var comment = new CommentCreateRequest
        {
            ArticleId = Guid.NewGuid(),
            Text = "@JohnDowe some text"
        };

        var callArgs = new List<NotificationCreateRequest>();

        _userRepositoryMock
            .Setup(ur => ur.GetUsersByLoginAsync(new List<string> { "JohnDowe" }, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User> { new User() });

        _notificationServiceMock
            .Setup(ns => ns.CreateAsync(It.IsAny<NotificationCreateRequest>(), It.IsAny<ICollection<User>>(), It.IsAny<CancellationToken>()))
            .Callback((NotificationCreateRequest ncr, ICollection<User> a, CancellationToken b) => callArgs.Add(ncr))
            .Returns(Task.CompletedTask);

        // act
        await _sut.CreateAsync(_user.Id, comment, default);

        // assert
        _repositoryManagerMock
            .Verify(rm => rm.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _notificationServiceMock
            .Verify(ns => ns.CreateAsync(It.IsAny<NotificationCreateRequest>(), It.IsAny<ICollection<User>>(), It.IsAny<CancellationToken>()), Times.Once);
    }




    private void ConfigureUserManagerMocking()
    {
        var store = new Mock<IUserStore<User>>();
        store
            .Setup(x => x.FindByIdAsync("123", CancellationToken.None))
            .ReturnsAsync(new User()
            {
                UserName = "test@email.com",
                Id = Guid.NewGuid()
            });

        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        //_userManagerMock
        //    .Setup(userManager => userManager.GetUsersInRoleAsync(UserRoles.Moderator.ToString()))
        //    .ReturnsAsync(new List<User>
        //    {
        //        new User{Id = Guid.NewGuid()},
        //        new User{Id = Guid.NewGuid()},
        //        new User{Id = Guid.NewGuid()}
        //    });
    }
}
