using System;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts.Repositories;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public class NotificationRepository : RepositoryBase<UserNotification, Guid>, INotificationRepository
{
    public NotificationRepository(ApplicationContext context) : base(context)
    {
    }


    public async Task<UserNotification?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetById(id, trackChanges)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ICollection<UserNotification>> GetUserNotificationsAsync(Guid userId, bool unreadOnly, bool trackChanges, CancellationToken cancellationToken)
    {
        var query = FindByCondition(n => n.UserId == userId && !n.Deleted);

        if (unreadOnly)
            query = query.Where(n => n.IsRead == false);

        return await query.ToListAsync(cancellationToken);
    }
}

