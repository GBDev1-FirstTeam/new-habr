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
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        return await GetAll(trackChanges).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Comment>> GetByArticleIdAsync(
        Guid articleId,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(comment => comment.ArticleId == articleId, trackChanges).ToListAsync(cancellationToken);
    }

    public async Task<Comment?> GetByIdAsync(
        Guid id,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        return await GetById(id, trackChanges).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Comment>> GetByUserIdAsync(
        Guid userId,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(comment => comment.UserId == userId, trackChanges).ToListAsync(cancellationToken);
    }

    public async Task<ICollection<UserComment>> GetUserCommentAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var response = await FindByCondition(comment => comment.UserId == userId && !comment.Deleted)
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

    public async Task<ICollection<UserLikedComment>> GetUserLikedCommentsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await FindByCondition(comment => !comment.Deleted)
            .Include(comment => comment.Article)
            .Include(comment => comment.Likes)
            .Where(comment => comment.Article.Published && !comment.Article.Deleted && comment.Likes.Any(like => like.UserId == userId))
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
