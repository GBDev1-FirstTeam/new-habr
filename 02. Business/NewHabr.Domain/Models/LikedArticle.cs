namespace NewHabr.Domain.Models;

public class LikedArticle
{
    public User User { get; set; } = null!;
    public Article Article { get; set; } = null!;
}
