using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

namespace NewHabr.Domain.Contracts;

public interface IArticleService
{
    /// <exception cref="ArticleNotFoundException"></exception>
    Task<ArticleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ArticleDto>> GetUnpublishedAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default);

    Task CreateAsync(CreateArticleRequest request, CancellationToken cancellationToken = default);

    /// <exception cref="ArticleNotFoundException"></exception>
    Task UpdateAsync(ArticleDto articleToUpdate, CancellationToken cancellationToken = default);

    /// <exception cref="ArticleNotFoundException"></exception>
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
