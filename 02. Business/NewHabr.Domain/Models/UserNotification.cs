#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class UserNotification : BaseEntity<Guid>
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public User User { get; set; }

    [Required]
    public string Text { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public bool IsRead { get; set; }
}
