using NewHabr.Domain.Dto;

namespace NewHabr.Domain;

public class PagedList<T> : List<T>
{
    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        Metadata = new PaginationMetadata
        {
            TotalCount = count,
            CurrentPage = pageNumber,
            PageSize = items.Count,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize),
        };
        AddRange(items);
    }

    public PaginationMetadata Metadata { get; }
}
