using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;
public interface ICommentRepository : IRepository<Comment>
{
    Task<IReadOnlyCollection<Comment>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Comment>> GetByArticleIdAsync(Guid articleId, CancellationToken cancellationToken = default);
}
