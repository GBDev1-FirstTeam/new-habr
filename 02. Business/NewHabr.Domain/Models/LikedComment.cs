#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class LikedComment
{
    // Who Liked a comment
    [Required]
    public Guid UserId { get; set; }

    public User User { get; set; }

    [Required]
    public Guid CommentId { get; set; }

    public Comment Comment { get; set; }
}
