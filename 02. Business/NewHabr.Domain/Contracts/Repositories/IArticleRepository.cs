using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IArticleRepository : IRepository<Article, Guid>
{
    Task<IReadOnlyCollection<Article>> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Article>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Article>> GetUnpublishedAsync(CancellationToken cancellationToken = default);
}
