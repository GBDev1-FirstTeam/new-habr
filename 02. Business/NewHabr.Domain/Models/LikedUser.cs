namespace NewHabr.Domain.Models;

public class LikedUser
{
    // Who Liked an article's author
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;
}
