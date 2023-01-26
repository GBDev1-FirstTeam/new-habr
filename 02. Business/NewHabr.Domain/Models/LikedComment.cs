namespace NewHabr.Domain.Models;

public class LikedComment
{
    public User User { get; set; } = null!;
    public Comment Comment { get; set; } = null!;
}
