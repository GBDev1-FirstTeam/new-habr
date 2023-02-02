using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IArticleService
{
    Task<IReadOnlyCollection<ArticleDto>> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ArticleDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ArticleDto>> GetPublishedAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default);
}
