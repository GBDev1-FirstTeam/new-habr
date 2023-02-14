using System;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts.Services;

public interface INotificationService
{
    /// <summary>
    /// Get user's notification 
    /// </summary>
    /// <param name="notificationId">Notification's Id</param>
    /// <param name="userId">User id requesting the notification</param>
    /// <returns></returns>
    Task<NotificationDto> GetByIdAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Create Notification for collection of users
    /// </summary>
    /// <param name="users">Collection of receivers</param>
    Task CreateAsync(NotificationCreateRequest request, ICollection<User> users, CancellationToken cancellationToken);

    /// <summary>
    /// Create Notification for collection of users
    /// </summary>
    /// <param name="users">Collection of receivers</param>
    Task CreateAsync(NotificationCreateRequest request, User user, CancellationToken cancellationToken);

    /// <summary>
    /// Mark notification as read
    /// </summary>
    /// <param name="notificationId">Notification's Id</param>
    /// <param name="userId">User id requesting the operation</param>
    Task MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Detele notification
    /// </summary>
    /// <param name="notificationId">Notification's Id</param>
    /// <param name="userId">User id requesting the operation</param>
    Task DeleteAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken);
}
