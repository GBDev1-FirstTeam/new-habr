using System;
using System.ComponentModel.Design;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class NotificationNotFoundException : NotFoundException
{
    public NotificationNotFoundException(Guid notifId)
        : base($"Notification with id: '{notifId}' not found")
    {
    }
}
