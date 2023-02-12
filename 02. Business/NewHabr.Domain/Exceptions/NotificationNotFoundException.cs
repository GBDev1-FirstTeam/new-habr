using System;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class NotificationNotFoundException : EntityNotFoundException
{
    public NotificationNotFoundException() : base(typeof(UserNotification))
    {
    }
}
