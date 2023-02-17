#nullable disable
namespace NewHabr.Domain.Dto;

public class ArticleGetResponse
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int PageCount { get; set; }
    public ICollection<ArticleDto> Articles { get; set; }
}
