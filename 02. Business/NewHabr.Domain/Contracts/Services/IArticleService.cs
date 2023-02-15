using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IArticleService
{
    /// <exception cref="ArticleNotFoundException"></exception>
    Task<IReadOnlyCollection<CommentWithLikedMark>> GetCommentsWithLikedMarkAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <exception cref="ArticleNotFoundException"></exception>
    Task<ArticleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ArticleDto>> GetUnpublishedAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default);

    /// <exception cref="CategoryNotFoundException"></exception>
    Task CreateAsync(ArticleCreateRequest request, Guid creatorId, CancellationToken cancellationToken = default);

    /// <exception cref="ArticleNotFoundException"></exception>
    Task UpdateAsync(Guid articleId, Guid modifierId, ArticleUpdateRequest articleToUpdate, CancellationToken cancellationToken = default);

    /// <exception cref="ArticleNotFoundException"></exception>
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <exception cref="ArticleNotFoundException"></exception>
    /// <exception cref="ArticleIsNotApproveException"></exception>
    Task SetPublicationStatusAsync(Guid id, bool publicationStatus, CancellationToken cancellationToken = default);

    /// <exception cref="ArticleNotFoundException"></exception>
    Task SetApproveStateAsync(Guid id, ApproveState state, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets 'Like' mark at article
    /// </summary>
    Task SetLikeAsync(Guid articleId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Unsets 'Like' mark at article
    /// </summary>
    Task UnsetLikeAsync(Guid articleId, Guid userId, CancellationToken cancellationToken);
}
