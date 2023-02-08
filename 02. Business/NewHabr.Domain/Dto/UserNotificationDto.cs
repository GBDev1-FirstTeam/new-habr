using System;
using NewHabr.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class UserNotificationDto
{
    public Guid UserId { get; set; }

    public string Text { get; set; }

    public long CreatedAt { get; set; }

    public bool IsRead { get; set; }
}
