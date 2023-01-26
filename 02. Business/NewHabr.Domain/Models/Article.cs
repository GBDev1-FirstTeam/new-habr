using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class Article : BaseEntity<Guid>
{
    [Required, MinLength(10), MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    public User User { get; set; } = null!;

    public ICollection<Category> Categories { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = null!;
    public ICollection<Tag> Tags { get; set; } = null!;
    public ICollection<LikedArticle> Likes { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }

    public bool Published { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }

    public bool Deleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public ApproveState ApproveState { get; set; }
}
