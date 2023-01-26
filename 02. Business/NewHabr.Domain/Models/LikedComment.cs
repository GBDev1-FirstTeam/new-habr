namespace NewHabr.Domain.Models;

public class LikedComment
{
    // Who Liked a comment
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid CommentId { get; set; }
    public Comment Comment { get; set; } = null!;
}
