#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class Comment : BaseEntity<Guid>
{
    [Required]
    public Guid UserId { get; set; }

    public User User { get; set; }

    [Required]
    public Guid ArticleId { get; set; }

    public Article Article { get; set; }

    [Required]
    public string Text { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<LikedComment> Likes { get; set; }
}
