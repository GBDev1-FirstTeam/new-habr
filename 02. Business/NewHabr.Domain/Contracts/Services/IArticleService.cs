using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IArticleService
{
    /// <exception cref="ArticleNotFoundException"></exception>
    Task<ArticleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ArticleDto>> GetUnpublishedAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default);

    /// <exception cref="CategoryNotFoundException"></exception>
    Task CreateAsync(CreateArticleRequest request, CancellationToken cancellationToken = default);

    /// <exception cref="ArticleNotFoundException"></exception>
    Task UpdateAsync(UpdateArticleRequest articleToUpdate, CancellationToken cancellationToken = default);

    /// <exception cref="ArticleNotFoundException"></exception>
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <exception cref="ArticleIsNotApproveException"></exception>
    /// <exception cref="ArticleNotFoundException"></exception>
    Task SetPublicationStatusAsync(Guid id, bool publicationStatus, CancellationToken cancellationToken = default);

    /// <exception cref="ArticleNotFoundException"></exception>
    Task SetApproveStateAsync(Guid id, ApproveState state, CancellationToken cancellationToken = default);
}
