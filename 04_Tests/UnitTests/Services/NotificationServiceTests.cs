using System;
using AutoFixture;
using Moq;
using NewHabr.Business.Services;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace UnitTests.Services;

public class NotificationServiceTests
{

    public class CreateAsync : ServiceTestsBase
    {
        private readonly INotificationService _sut;


        public CreateAsync()
        {
            _sut = new NotificationService(
                repositoryManager: _repositoryManagerMock.Object,
                mapper: _mapper,
                logger: null);
        }


        [Fact]
        public async Task Should_create_valid_notification()
        {
            // Arrange
            var request = _fixture
                .Create<NotificationCreateRequest>();
            var user = _fixture
                .Build<User>()
                .OmitAutoProperties()
                .With(user => user.Id)
                .With(user => user.UserName)
                .Create();
            Notification createdNotification = null;
            _notificationRepositoryMock
                .Setup(r => r.Create(It.IsAny<Notification>()))
                .Callback((Notification n) => createdNotification = n);

            // Act
            await _sut.CreateAsync(request, user, default);

            // Assert
            Assert.NotNull(createdNotification);
            Assert.Equal(request.Text, createdNotification.Text);
            Assert.False(createdNotification.IsRead);
            Assert.Equal(1, createdNotification.Users.Count);
            Assert.Equal(DateTimeOffset.UtcNow.UtcDateTime, createdNotification.CreatedAt.UtcDateTime, TimeSpan.FromMinutes(1));
            _notificationRepositoryMock
                .Verify(r => r.Create(It.IsAny<Notification>()), Times.Once);
            _repositoryManagerMock
                .Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    public class DeleteAsync : ServiceTestsBase
    {
        private readonly INotificationService _sut;
        private User _userRequestedDeletion;
        private Notification _notification;

        public DeleteAsync()
        {
            _userRequestedDeletion = _fixture
                .Build<User>()
                .OmitAutoProperties()
                .With(user => user.Id)
                .With(user => user.UserName)
                .Create();

            _notification = _fixture
                .Build<Notification>()
                .With<ICollection<User>>(u => u.Users, new List<User> { _userRequestedDeletion })
                .Create();

            _sut = new NotificationService(
                repositoryManager: _repositoryManagerMock.Object,
                mapper: _mapper,
                logger: null);
        }


        [Fact]
        public async Task SHOULD_call_delete_method_WHEN_request_valid()
        {
            // Arrange
            Notification deletedNotification = null;
            _notificationRepositoryMock
                .Setup(r => r.GetByIdAsync(_notification.Id, _userRequestedDeletion.Id, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_notification);
            _notificationRepositoryMock
                .Setup(r => r.Delete(It.IsAny<Notification>()))
                .Callback((Notification n) => deletedNotification = n);

            // Act
            await _sut.DeleteAsync(_notification.Id, _userRequestedDeletion.Id, default);

            // Assert
            Assert.NotNull(deletedNotification);
            Assert.Contains(deletedNotification.Users, owner => owner.Id.Equals(_userRequestedDeletion.Id));
            _notificationRepositoryMock
                .Verify(r => r.Delete(It.IsAny<Notification>()), Times.Once);
            _repositoryManagerMock
                .Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SHOULD_throw_NotificationNotFoundException_WHEN_notification_not_found()
        {
            // Arrange
            _notificationRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Notification)null!);

            // Act
            var Act = () => _sut.DeleteAsync(_notification.Id, _userRequestedDeletion.Id, default);

            // Assert
            await Assert.ThrowsAsync<NotificationNotFoundException>(Act);
            _notificationRepositoryMock
                .Verify(r => r.Delete(It.IsAny<Notification>()), Times.Never);
            _repositoryManagerMock
                .Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }

    public class MarkAsReadAsync : ServiceTestsBase
    {
        private readonly INotificationService _sut;
        private User _userRequestedAction;
        private Notification _notification;


        public MarkAsReadAsync()
        {
            _sut = new NotificationService(
                repositoryManager: _repositoryManagerMock.Object,
                mapper: _mapper,
                logger: null);

            _userRequestedAction = _fixture
                .Build<User>()
                .OmitAutoProperties()
                .With(user => user.Id)
                .With(user => user.UserName)
                .Create();

            _notification = _fixture
                .Build<Notification>()
                .With<ICollection<User>>(u => u.Users, new List<User> { _userRequestedAction })
                .With(n => n.IsRead, false)
                .Create();
        }


        [Fact]
        public async Task SHOULD_mark_as_read_WHEN_valid_request()
        {
            // Arrange
            _notificationRepositoryMock
                .Setup(r => r.GetByIdAsync(_notification.Id, _userRequestedAction.Id, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_notification);
            // Act
            await _sut.MarkAsReadAsync(_notification.Id, _userRequestedAction.Id, default);

            // Assert
            Assert.True(_notification.IsRead);
            _repositoryManagerMock
                .Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SHOULD_throw_NotificationNotFoundException_WHEN_notification_not_found()
        {
            // Arrange
            _notificationRepositoryMock
                .Setup(r => r.GetByIdAsync(_notification.Id, _userRequestedAction.Id, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Notification)null!);
            // Act
            var Act = () => _sut.MarkAsReadAsync(_notification.Id, _userRequestedAction.Id, default);

            // Assert
            await Assert.ThrowsAsync<NotificationNotFoundException>(Act);
            _repositoryManagerMock
                .Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}

