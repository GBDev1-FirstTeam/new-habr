namespace NewHabr.Domain.Dto;

#nullable disable
public class CommentDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
    public string Text { get; set; }
    public long CreatedAt { get; set; }
}
