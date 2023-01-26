using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class Comment : BaseEntity<Guid>
{
    [Required]
    public User User { get; set; } = null!;

    [Required]
    public Article Article { get; set; } = null!;

    [Required]
    public string Text { get; set; } = string.Empty;

    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<LikedComment> Likes { get; set; } = null!;
}
