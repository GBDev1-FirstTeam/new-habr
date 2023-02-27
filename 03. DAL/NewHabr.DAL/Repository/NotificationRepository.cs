using System;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts.Repositories;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public class NotificationRepository : RepositoryBase<Notification, Guid>, INotificationRepository
{
    public NotificationRepository(ApplicationContext context) : base(context)
    {
    }


    public async Task<Notification?> GetByIdAsync(Guid notificationId, Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetById(notificationId, trackChanges)
            .Include(n => n.Users.Where(u => u.Id == userId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ICollection<Notification>> GetUserNotificationsAsync(Guid userId, bool unreadOnly, bool trackChanges, CancellationToken cancellationToken)
    {
        var query = GetAll(trackChanges);

        if (unreadOnly)
            query = query.Where(n => n.IsRead == false);

        var notifications = await query
            .Include(n => n.Users.Where(u => u.Id == userId))
            .ToListAsync(cancellationToken);

        return notifications;
    }
}

