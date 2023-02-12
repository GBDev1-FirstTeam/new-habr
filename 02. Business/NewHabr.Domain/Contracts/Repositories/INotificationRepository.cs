using System;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts.Repositories;

public interface INotificationRepository : IRepository<UserNotification, Guid>
{
    Task<UserNotification?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);
    Task<ICollection<UserNotification>> GetUserNotificationsAsync(Guid userId, bool unreadOnly, bool trackChanges, CancellationToken cancellationToken);
}

