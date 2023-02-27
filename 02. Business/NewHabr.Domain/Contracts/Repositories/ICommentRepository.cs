using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ICommentRepository : IRepository<Comment, Guid>
{
    Task<IReadOnlyCollection<Comment>> GetByUserIdAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Comment>> GetByArticleIdAsync(Guid articleId, bool trackChanges, CancellationToken cancellationToken);

    Task<ICollection<UserComment>> GetUserCommentAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);

    Task<ICollection<UserLikedComment>> GetUserLikedCommentsAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);

    Task<Comment?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Comment>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken);

    Task<Comment?> GetByIdWithLikesAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);
}
