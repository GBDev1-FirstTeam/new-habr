using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IArticleRepository : IRepository<Article>
{
    /// <summary></summary>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    /// <returns></returns>
    Task<IReadOnlyCollection<Article>> GetByTitleAsync(string title, CancellationToken cancellationToken = default);

    /// <summary></summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    /// <returns></returns>
    Task<IReadOnlyCollection<Article>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary></summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    /// <returns></returns>
    Task<IReadOnlyCollection<Article>> GetPublishedAsync(CancellationToken cancellationToken = default);

    /// <summary></summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    /// <returns></returns>
    Task<IReadOnlyCollection<Article>> GetDeletedAsync(CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
