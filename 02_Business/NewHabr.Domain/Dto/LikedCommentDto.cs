#nullable disable

namespace NewHabr.Domain.Dto;

public class LikedCommentDto
{
    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
    public string ArticleTitle { get; set; }
    public string Text { get; set; }
}
