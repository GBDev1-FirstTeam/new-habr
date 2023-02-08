using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository.Impl;
public class CommentRepository : ReporitoryBase<Comment, Guid>, ICommentRepository
{

    public CommentRepository(ApplicationContext context) : base(context)
    {
    }

    public Task<IReadOnlyCollection<Comment>> GetByArticleIdAsync(Guid articleId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Comment>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<UserComment>> GetUserCommentAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var response = await FindByCondition(comment => comment.UserId == userId && !comment.Deleted)
            .Select(row => new UserComment
            {
                Id = row.Id,
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
