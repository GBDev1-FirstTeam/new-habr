using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class QueryParameters
{
    [Range(1, int.MaxValue)]
    public virtual int PageNumber { get; set; } = 1;

    [Range(1, int.MaxValue)]
    public virtual int PageSize { get; set; } = 10;
}
