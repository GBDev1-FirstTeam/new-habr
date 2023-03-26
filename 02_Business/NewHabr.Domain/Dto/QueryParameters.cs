using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NewHabr.Domain.Dto;

public class QueryParameters
{
    [FromQuery(Name = QueryParametersDefinitions.PageNumber)]
    [Range(1, int.MaxValue)]
    public virtual int PageNumber { get; set; } = 1;

    [FromQuery(Name = QueryParametersDefinitions.PageSize)]
    [Range(1, 30)]
    public virtual int PageSize { get; set; } = 10;
}
