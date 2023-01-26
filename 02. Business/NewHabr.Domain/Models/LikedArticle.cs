namespace NewHabr.Domain.Models;

public class LikedArticle
{
    // Who Liked an article
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid ArticleId { get; set; }
    public Article Article { get; set; } = null!;
}
