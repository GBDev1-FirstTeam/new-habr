#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class UserNotificationCreateRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string Text { get; set; }
}
