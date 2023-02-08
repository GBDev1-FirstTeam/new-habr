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
}
