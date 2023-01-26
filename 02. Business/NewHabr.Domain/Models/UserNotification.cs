namespace NewHabr.Domain.Models;

public class UserNotification : BaseEntity<Guid>
{
    public User User { get; set; } = null!;
    public string Text { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
    public bool IsRead { get; set; }
}
