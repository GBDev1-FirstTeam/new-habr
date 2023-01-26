namespace NewHabr.Domain.Dto;

public class ArticleDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public long CreatedAt { get; set; }
    public long ModifyAt { get; set; }
    public long PublishedAt { get; set; }
}
