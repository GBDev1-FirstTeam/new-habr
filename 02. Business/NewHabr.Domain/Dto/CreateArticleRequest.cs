#nullable disable
namespace NewHabr.Domain.Dto;

public class CreateArticleRequest
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}
