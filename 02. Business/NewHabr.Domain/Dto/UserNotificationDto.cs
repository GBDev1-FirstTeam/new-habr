#nullable disable

namespace NewHabr.Domain.Dto;

public class UserNotificationDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Text { get; set; }

    public long CreatedAt { get; set; }

    public bool IsRead { get; set; }
}
