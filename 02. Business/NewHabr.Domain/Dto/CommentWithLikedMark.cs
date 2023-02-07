#nullable disable
namespace NewHabr.Domain.Dto;

public class CommentWithLikedMark
{
    public Guid UserId { get; set; }
    public CommentDto Comment { get; set; }
    public bool IsLiked { get; set; }
}
