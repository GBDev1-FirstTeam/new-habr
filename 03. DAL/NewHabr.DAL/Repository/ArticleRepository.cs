using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public class ArticleRepository : ReporitoryBase<Article, Guid>, IArticleRepository
{
    public ArticleRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<Article>> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(a => a.Title.ToLower() == title.ToLower() && !a.Deleted).ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyCollection<Article>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(a => a.UserId == userId && !a.Deleted).ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyCollection<Article>> GetUnpublishedAsync(CancellationToken cancellationToken = default)
    {
        return await FindByCondition(a => !a.Published && !a.Deleted).ToListAsync(cancellationToken);
    }
}
