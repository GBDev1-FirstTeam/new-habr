using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NewHabr.Domain.Dto;

public class ArticleQueryParametersDto : QueryParameters
{
    [FromQuery(Name = QueryParametersDefinitions.FromDateTime)]
    [Range(0, long.MaxValue)]
    public long From { get; set; }

    [FromQuery(Name = QueryParametersDefinitions.ToDateTime)]
    [Range(0, long.MaxValue)]
    public long To { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    [FromQuery(Name = QueryParametersDefinitions.OrderBy)]
    [StringLength(10)]
    public string OrderBy { get; set; } = QueryParametersDefinitions.OrderingTypes.Descending;
}
