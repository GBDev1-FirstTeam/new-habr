#nullable disable

using static NewHabr.Domain.QueryParametersDefinitions;

namespace NewHabr.Domain.Dto;

public class ArticleQueryParameters : QueryParameters
{
    public DateTimeOffset From { get; set; }

    public DateTimeOffset To { get; set; } = DateTimeOffset.UtcNow;

    public string OrderBy { get; set; }

    public RatingOrderBy ByRating { get; set; }
}
