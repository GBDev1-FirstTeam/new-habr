using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.WebApi.Extensions;

namespace NewHabr.WebApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;


    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }


    /// <summary>
    /// Retrieves a specific Notification
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult<NotificationDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var notification = await _notificationService.GetByIdAsync(id, userId, cancellationToken);
        return Ok(notification);

    }

    /// <summary>
    /// Marks a specific Notification as read
    /// </summary>
    [HttpPut("{id}/markAsRead")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult> MarkAsRead([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var notification = await _notificationService.GetByIdAsync(id, userId, cancellationToken);
        await _notificationService.MarkAsReadAsync(id, userId, cancellationToken);
        return NoContent();

    }

    /// <summary>
    /// Deletes a specific Notification
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await _notificationService.DeleteAsync(id, userId, cancellationToken);
        return NoContent();

    }
}
