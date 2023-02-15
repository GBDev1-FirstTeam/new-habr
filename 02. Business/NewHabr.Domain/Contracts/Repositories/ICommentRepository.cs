using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ICommentRepository : IRepository<Comment, Guid>
{
    Task<IReadOnlyCollection<Comment>> GetByUserIdAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Comment>> GetByArticleIdAsync(Guid articleId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ICollection<UserComment>> GetUserCommentAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<ICollection<UserLikedComment>> GetUserLikedCommentsAsync(Guid userId, CancellationToken cancellationToken);

    Task<Comment?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Comment>> GetAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default);

    Task<Comment?> GetByIdWithLikesAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default);
}
