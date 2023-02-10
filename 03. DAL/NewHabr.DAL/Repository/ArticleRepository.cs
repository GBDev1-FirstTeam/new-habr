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

    public async Task<IReadOnlyCollection<Article>> GetByTitleIncludeAsync(
        string title,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(a => a.Title.ToLower() == title.ToLower() && !a.Deleted, trackChanges)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Article>> GetByUserIdIncludeAsync(
        Guid userId,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(a => a.UserId == userId && !a.Deleted, trackChanges)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Article>> GetUnpublishedIncludeAsync(
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(a => !a.Published && !a.Deleted, trackChanges)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Article>> GetDeletedIncludeAsync(
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        return await GetDeleted(trackChanges)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments)
            .ToListAsync(cancellationToken);
    }

    public async Task<Article?> GetByIdAsync(
        Guid id,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        return await GetById(id, trackChanges).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Article?> GetByIdIncludeAsync(
        Guid id,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(a => a.Id == id && !a.Deleted, trackChanges)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Article?> GetByIdIncludeCommentLikesAsync(
        Guid id,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(a => a.Id == id && !a.Deleted, trackChanges)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments).ThenInclude(c => c.Likes)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
