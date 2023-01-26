namespace NewHabr.Domain.Models;

public class Article : BaseEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public List<Category> Categories { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();
    public List<LikedArticle> Likes { get; set; } = new();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }

    public bool Published { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }

    public bool Deleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public ApproveState ApproveState { get; set; }
}
