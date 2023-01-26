namespace NewHabr.Domain.Models;

public class Comment : BaseEntity<Guid>
{
    public User User { get; set; } = null!;
    public Article Article { get; set; } = null!;
    public string Text { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public List<LikedComment> Likes { get; set; } = new();
}
