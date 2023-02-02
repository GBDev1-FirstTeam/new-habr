using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public class ArticleRepository : ReporitoryBase<Article, Guid>, IArticleRepository
{
    public ArticleRepository(ApplicationContext context, CancellationToken cancellationToken = default) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<Article>> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentException(nameof(title));
        }

        return await FindByCondition(a => a.Title.ToLower() == title.ToLower() && !a.Deleted).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Article>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(a => a.UserId == userId && !a.Deleted).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Article>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await FindByCondition(a => a.Deleted).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Article>> GetPublishedAsync(CancellationToken cancellationToken = default)
    {
        return await FindByCondition(a => a.Published && !a.Deleted).ToListAsync(cancellationToken);
    }
}
