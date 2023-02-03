using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IArticleRepository : IRepository<Article>
{
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<IReadOnlyCollection<Article>> GetByTitleAsync(string title, CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<IReadOnlyCollection<Article>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<IReadOnlyCollection<Article>> GetPublishedAsync(CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<IReadOnlyCollection<Article>> GetDeletedAsync(CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
