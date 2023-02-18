#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class Article : BaseEntity<Guid>
{
    [Required, MinLength(10), MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    public Guid UserId { get; set; }

    public User User { get; set; }

    public ICollection<Category> Categories { get; set; } = new HashSet<Category>();

    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

    public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

    public ICollection<User> Likes { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }

    public bool Published { get; set; }

    public DateTimeOffset? PublishedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public ApproveState ApproveState { get; set; }
}
