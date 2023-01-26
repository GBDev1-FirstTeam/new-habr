using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class UserNotification : BaseEntity<Guid>
{
    [Required]
    public User User { get; set; } = null!;

    [Required]
    public string Text { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
    public bool IsRead { get; set; }
}
