#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class NotificationCreateRequest
{
    [Required]
    public string Text { get; set; }
}
