using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Exceptions;
using NewHabr.WebApi.Extensions;

namespace NewHabr.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;


    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }


    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var notification = await _notificationService.GetByIdAsync(id, userId, cancellationToken);
        return Ok(notification);

    }

    [HttpPut("{id}/markAsRead")]
    [Authorize]
    public async Task<IActionResult> MarkAsRead([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var notification = await _notificationService.GetByIdAsync(id, userId, cancellationToken);
        await _notificationService.MarkAsReadAsync(id, userId, cancellationToken);
        return NoContent();

    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await _notificationService.DeleteAsync(id, userId, cancellationToken);
        return NoContent();

    }
}
