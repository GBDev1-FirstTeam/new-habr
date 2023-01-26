#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class LikedUser
{
    // Who Liked an article's author
    [Required]
    public Guid UserId { get; set; }

    public User User { get; set; }

    [Required]
    public Guid AuthorId { get; set; }

    public User Author { get; set; }
}
