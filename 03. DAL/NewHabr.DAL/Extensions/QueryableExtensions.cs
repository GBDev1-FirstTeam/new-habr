using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NewHabr.Domain;

namespace NewHabr.DAL.Extensions;
public static class QueryableExtensions
{
    public static async Task<PagedList<TEntity>> ToPagedListAsync<TEntity>(
        this IQueryable<TEntity> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var count = source.Count();
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return new PagedList<TEntity>(items, count, pageNumber, pageSize);
    }

    public static IOrderedQueryable<TEntity> OrderByType<TEntity, TResult>(
        this IQueryable<TEntity> source,
        Expression<Func<TEntity, TResult>> keySelector,
        string orderingType)
    {
        if (!string.IsNullOrWhiteSpace(orderingType))
        {
            orderingType = orderingType.Trim().ToLower();
        }

        return orderingType switch
        {
            QueryParametersDefinitions.OrderingTypes.Ascending => source.OrderBy(keySelector),
            QueryParametersDefinitions.OrderingTypes.Descending => source.OrderByDescending(keySelector),
            _ => source.OrderByDescending(keySelector),
        };
    }
}
