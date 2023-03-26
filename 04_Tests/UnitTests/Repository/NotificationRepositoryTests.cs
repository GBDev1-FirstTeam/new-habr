using System;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NewHabr.Business.Services;
using NewHabr.DAL.EF;
using NewHabr.DAL.Repository;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Repositories;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace UnitTests.Repository;

public class NotificationRepositoryTests
{
    public class GetByIdAsync : RepositoryTestsBase
    {
        private INotificationRepository _sut;
        private AutoFixture.Dsl.IPostprocessComposer<User> _userBuilder;


        public GetByIdAsync()
        {
            _userBuilder = _fixture
                .Build<User>()
                .OmitAutoProperties()
                .With(user => user.Id)
                .With(user => user.UserName)
                .With(user => user.SecureQuestion)
                .With(user => user.SecureAnswer);

            _sut = new NotificationRepository(_context);
        }


        [Fact]
        public async Task SHOULD_return_notification_WHEN_request_is_validAsync()
        {
            // Arrange
            var user = _userBuilder.Create();
            var notification = _fixture
                .Build<Notification>()
                .With<ICollection<User>>(u => u.Users, new List<User> { user })
                .With(n => n.Deleted, false)
                .Create();
            _context
                .Set<Notification>()
                .Add(notification);
            _context.SaveChanges();

            // Act
            var foundNotification = await _sut.GetByIdAsync(notification.Id, user.Id, false, default);

            // Assert
            Assert.NotNull(foundNotification);
        }

        [Fact]
        public async Task SHOULD_return_null_WHEN_user_not_own_notification()
        {
            // Arrange
            var user = _userBuilder.Create();
            var notification = _fixture
                .Build<Notification>()
                .With<ICollection<User>>(u => u.Users, new List<User> { user })
                .With(n => n.Deleted, false)
                .Create();
            _context
                .Set<Notification>()
                .Add(notification);
            _context.SaveChanges();

            // Act
            var foundNotification = await _sut.GetByIdAsync(notification.Id, Guid.NewGuid(), false, default);

            // Assert
            Assert.Null(foundNotification);
        }

        [Fact]
        public async Task SHOULD_return_null_WHEN_notification_not_foundAsync()
        {
            // Arrange
            var user = _userBuilder.Create();
            var notification = _fixture
                .Build<Notification>()
                .With<ICollection<User>>(u => u.Users, new List<User> { user })
                .With(n => n.Deleted, false)
                .Create();
            _context
                .Set<Notification>()
                .Add(notification);
            _context.SaveChanges();

            // Act
            var foundNotification = await _sut.GetByIdAsync(Guid.NewGuid(), Guid.NewGuid(), false, default);

            // Assert
            Assert.Null(foundNotification);
        }
    }

    public class GetUserNotificationsAsync : RepositoryTestsBase
    {
        private AutoFixture.Dsl.IPostprocessComposer<User> _userBuilder;
        private INotificationRepository _sut;


        public GetUserNotificationsAsync()
        {
            _sut = new NotificationRepository(_context);
            _userBuilder = _fixture
                .Build<User>()
                .OmitAutoProperties()
                .With(user => user.Id)
                .With(user => user.UserName)
                .With(user => user.SecureQuestion)
                .With(user => user.SecureAnswer);

        }


        [Fact]
        public async Task SHOULD_return_empty_collection_WHEN_nothing_found()
        {
            // Arrange
            var otherNotifications = _fixture
                .Build<Notification>()
                .Without(u => u.Id)
                .With<ICollection<User>>(u => u.Users, _userBuilder.CreateMany(5).ToList())
                .With(n => n.Deleted, false)
                .CreateMany(50);
            _context
                .Set<Notification>()
                .AddRange(otherNotifications);
            _context.SaveChanges();

            // Act
            var notificationsFromDb = await _sut.GetUserNotificationsAsync(Guid.NewGuid(), false, false, default);

            // Assert
            Assert.NotNull(notificationsFromDb);
            Assert.Equal(0, notificationsFromDb.Count);
        }

        [Fact]
        public async void SHOULD_return_notifications_WHEN_request_valid()
        {
            // Arrange
            var expectedUser = _userBuilder.Create();
            var expectedNotifications = _fixture
                .Build<Notification>()
                .With<ICollection<User>>(u => u.Users, new List<User> { expectedUser })
                .With(n => n.Deleted, false)
                .CreateMany();
            _context
                .Set<Notification>()
                .AddRange(expectedNotifications);
            _context.SaveChanges();

            var notifications = _fixture
                .Build<Notification>()
                .With<ICollection<User>>(u => u.Users, _userBuilder.CreateMany().ToList())
                .With(n => n.Deleted, false)
                .CreateMany();
            _context
                .Set<Notification>()
                .AddRange(notifications);
            _context.SaveChanges();

            // Act
            var notificationsFromDb = await _sut.GetUserNotificationsAsync(expectedUser.Id, false, false, default);

            // Assert
            Assert.NotNull(notificationsFromDb);
            Assert.Equal(expectedNotifications.Count(), notificationsFromDb.Count);
            Assert.Contains(notificationsFromDb, n => expectedNotifications.Select(en => en.Id).Contains(n.Id));
        }

        [Fact]
        public async void SHOULD_return_unread_notifications_WHEN_requested_unread_only()
        {
            // Arrange
            var expectedUser = _userBuilder.Create();
            var expectedNotifications = _fixture
                .Build<Notification>()
                .Without(u => u.Users)
                .With(n => n.Deleted, false)
                .With(n => n.IsRead, false)
                .CreateMany();

            var readNotifications = _fixture
                .Build<Notification>()
                .Without(u => u.Users)
                .With(n => n.Deleted, false)
                .With(n => n.IsRead, true)
                .CreateMany();

            var list = new List<Notification>(expectedNotifications);
            list.AddRange(readNotifications);
            expectedUser.Notifications = list;
            _context
                .Set<User>()
                .AddRange(expectedUser);

            var otherNotifications = _fixture
                .Build<Notification>()
                .Without(u => u.Id)
                .With<ICollection<User>>(u => u.Users, _userBuilder.CreateMany(5).ToList())
                .With(n => n.Deleted, false)
                .CreateMany(50);
            _context
                .Set<Notification>()
                .AddRange(otherNotifications);
            _context.SaveChanges();

            // Act
            var notificationsFromDb = await _sut.GetUserNotificationsAsync(expectedUser.Id, true, false, default);

            // Assert
            Assert.NotNull(notificationsFromDb);
            Assert.Equal(expectedNotifications.Count(), notificationsFromDb.Count);
            Assert.Contains(notificationsFromDb, n => expectedNotifications.Select(en => en.Id).Contains(n.Id));
        }
    }
}
