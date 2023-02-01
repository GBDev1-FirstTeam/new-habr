using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewHabr.DAL.EF;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository.Impl;
public class CommentRepository : ReporitoryBase<Comment,Guid>, ICommentRepository
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
}
