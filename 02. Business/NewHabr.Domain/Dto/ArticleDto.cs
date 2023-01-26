namespace NewHabr.Domain.Dto;

#nullable disable
public class ArticleDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public long CreatedAt { get; set; }
    public long ModifiedAt { get; set; }
    public long PublishedAt { get; set; }
}
