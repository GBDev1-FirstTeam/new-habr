using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IReadOnlyCollection<Comment>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Comment>> GetByArticleIdAsync(Guid articleId, CancellationToken cancellationToken = default);
}
