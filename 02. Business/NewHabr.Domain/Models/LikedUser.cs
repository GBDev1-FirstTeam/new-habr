namespace NewHabr.Domain.Models;

public class LikedUser
{
    public User User { get; set; } = null!;
    public User Author { get; set; } = null!;
}
