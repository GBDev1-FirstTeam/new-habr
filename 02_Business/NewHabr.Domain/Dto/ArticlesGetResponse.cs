namespace NewHabr.Domain.Dto;

public class ArticlesGetResponse
{
    public PaginationMetadata Metadata { get; set; }

    public ICollection<ArticleDto> Articles { get; set; }
}
