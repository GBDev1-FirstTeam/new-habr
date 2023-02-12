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


    public async Task CreateAsync(UserNotificationCreateRequest request, CancellationToken cancellationToken)
    {
        var userNotification = _mapper.Map<UserNotification>(request);
        userNotification.CreatedAt = DateTimeOffset.UtcNow;
        userNotification.IsRead = false;

        _repositoryManager.NotificationRepository.Create(userNotification);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken)
    {
        var notification = await GetNotificationAndCheckIfItExistsAsync(notificationId, true, cancellationToken);

        if (notification.UserId != userId)
        {
            _logger.LogInformation($"User with id {userId} tried to delete notification with id {notification.Id}");
            throw new NotificationNotFoundException();
        }

        _repositoryManager
            .NotificationRepository
            .Delete(notification);

        await _repositoryManager
            .SaveAsync(cancellationToken);
    }

    public async Task<UserNotificationDto> GetByIdAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken)
    {
        var notification = await GetNotificationAndCheckIfItExistsAsync(notificationId, false, cancellationToken);

        if (notification.UserId != userId)
        {
            _logger.LogInformation($"User with id {userId} tried to access notification with id {notification.Id}");
            throw new NotificationNotFoundException();
        }

        var dto = _mapper.Map<UserNotificationDto>(notification);
        return dto;
    }

    public async Task MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken)
    {
        var notification = await GetNotificationAndCheckIfItExistsAsync(notificationId, true, cancellationToken);

        if (notification.UserId != userId)
        {
            _logger.LogInformation($"User with id {userId} tried to mark as read notification with id {notification.Id}");
            throw new NotificationNotFoundException();
        }

        notification.IsRead = true;
        await _repositoryManager.SaveAsync(cancellationToken);
    }


    private async Task<UserNotification> GetNotificationAndCheckIfItExistsAsync(Guid id, bool trackChanges, CancellationToken cancellationToken)
    {
        var notification = await _repositoryManager
            .NotificationRepository
            .GetByIdAsync(id, trackChanges, cancellationToken);

        if (notification is null)
        {
            throw new NotificationNotFoundException();
        }

        return notification;
    }
}
