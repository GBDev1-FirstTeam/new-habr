#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class LikedArticle
{
    // Who Liked an article
    [Required]
    public Guid UserId { get; set; }

    public User User { get; set; }

    [Required]
    public Guid ArticleId { get; set; }

    public Article Article { get; set; }
}
