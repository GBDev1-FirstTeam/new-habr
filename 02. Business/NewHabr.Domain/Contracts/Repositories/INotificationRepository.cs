using System;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts.Repositories;

public interface INotificationRepository : IRepository<Notification, Guid>
{
    Task<Notification?> GetByIdAsync(Guid notificationId, Guid userId, bool trackChanges, CancellationToken cancellationToken);
    Task<ICollection<Notification>> GetUserNotificationsAsync(Guid userId, bool unreadOnly, bool trackChanges, CancellationToken cancellationToken);
}

