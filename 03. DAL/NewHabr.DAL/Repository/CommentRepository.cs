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

    public async Task<IReadOnlyCollection<Comment>> GetByArticleIdAsync(Guid articleId, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(comment => comment.ArticleId == articleId, trackChanges).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Comment>> GetByUserIdAsync(Guid userId, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(comment => comment.UserId == userId, trackChanges).ToListAsync(cancellationToken);
    }
}
