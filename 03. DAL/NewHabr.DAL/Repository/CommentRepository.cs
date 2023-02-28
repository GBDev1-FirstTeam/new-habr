using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository.Impl;
public class CommentRepository : RepositoryBase<Comment, Guid>, ICommentRepository
{

    public CommentRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<Comment>> GetAllAsync(
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        return await GetAll(trackChanges).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Comment>> GetByArticleIdAsync(
        Guid articleId,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        return await FindByCondition(comment => comment.ArticleId == articleId, trackChanges).ToListAsync(cancellationToken);
    }

    public async Task<Comment?> GetByIdAsync(
        Guid id,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        return await GetById(id, trackChanges).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Comment?> GetByIdWithLikesAsync(Guid id, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetById(id, trackChanges)
            .Include(c => c.Likes)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Comment>> GetByUserIdAsync(
        Guid userId,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        return await FindByCondition(comment => comment.UserId == userId, trackChanges).ToListAsync(cancellationToken);
    }

    public async Task<ICollection<UserComment>> GetUserCommentAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        var response = await FindByCondition(comment => comment.UserId == userId, trackChanges)
            .Select(row => new UserComment
            {
                Id = row.Id,
                UserId = userId,
                ArticleId = row.ArticleId,
                Text = row.Text,
                CreatedAt = row.CreatedAt,
                LikesCount = row.Likes.Count()
            })
            .ToListAsync(cancellationToken);
        return response;
    }

    public async Task<ICollection<UserLikedComment>> GetUserLikedCommentsAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetAll(trackChanges)
            .Include(comment => comment.Article)
            .Include(comment => comment.Likes)
            .Where(comment => comment.Article.Published && comment.Likes.Any(u => u.Id == userId))
            .Select(row => new UserLikedComment
            {
                Id = row.Id,
                ArticleId = row.ArticleId,
                ArticleTitle = row.Article.Title,
                Text = row.Text
            })
            .ToListAsync(cancellationToken);
    }
}
