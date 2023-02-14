using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace NewHabr.Business.Services;

public class NotificationService : INotificationService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(IRepositoryManager repositoryManager, IMapper mapper, ILogger<NotificationService> logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }


    public async Task CreateAsync(NotificationCreateRequest request, ICollection<User> users, CancellationToken cancellationToken)
    {
        var notification = _mapper.Map<Notification>(request);
        notification.CreatedAt = DateTimeOffset.UtcNow;
        notification.IsRead = false;
        notification.Users = new List<User>(users);

        _repositoryManager.NotificationRepository.Create(notification);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task CreateAsync(NotificationCreateRequest request, User user, CancellationToken cancellationToken)
    {
        var users = new List<User>() { user };
        await CreateAsync(request, users, cancellationToken);
    }

    public async Task DeleteAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken)
    {
        var notification = await GetUserNotificationAndCheckIfItExistsAsync(notificationId, userId, true, cancellationToken);

        _repositoryManager
            .NotificationRepository
            .Delete(notification);

        await _repositoryManager
            .SaveAsync(cancellationToken);
    }

    public async Task<NotificationDto> GetByIdAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken)
    {
        var notification = await GetUserNotificationAndCheckIfItExistsAsync(notificationId, userId, false, cancellationToken);

        var dto = _mapper.Map<NotificationDto>(notification);
        return dto;
    }

    public async Task MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken)
    {
        var notification = await GetUserNotificationAndCheckIfItExistsAsync(notificationId, userId, true, cancellationToken);

        notification.IsRead = true;
        await _repositoryManager.SaveAsync(cancellationToken);
    }


    private async Task<Notification> GetUserNotificationAndCheckIfItExistsAsync(Guid notificationId, Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        var notification = await _repositoryManager
            .NotificationRepository
            .GetByIdAsync(notificationId, userId, trackChanges, cancellationToken);

        if (notification is null)
        {
            throw new NotificationNotFoundException();
        }

        return notification;
    }
}
