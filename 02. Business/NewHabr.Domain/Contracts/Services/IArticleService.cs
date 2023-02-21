using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IArticleService
{
    Task<IReadOnlyCollection<CommentWithLikedMark>> GetCommentsWithLikedMarkAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken);

    Task<ArticleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ArticlesGetResponse> GetPublishedAsync(ArticleQueryParametersDto queryParams, CancellationToken cancellationToken);

    Task<ArticlesGetResponse> GetUnpublishedAsync(ArticleQueryParametersDto queryParams, CancellationToken cancellationToken);

    Task<ArticlesGetResponse> GetDeletedAsync(ArticleQueryParametersDto queryParams, CancellationToken cancellationToken);

    Task CreateAsync(ArticleCreateRequest request, Guid creatorId, CancellationToken cancellationToken);

    Task UpdateAsync(Guid articleId, Guid modifierId, ArticleUpdateRequest articleToUpdate, CancellationToken cancellationToken);

    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);

    Task SetPublicationStatusAsync(Guid id, bool publicationStatus, CancellationToken cancellationToken);

    /// <exception cref="ArticleNotFoundException"></exception>
    Task SetApproveStateAsync(Guid id, ApproveState state, CancellationToken cancellationToken);

    /// <summary>
    /// Sets 'Like' mark at article
    /// </summary>
    Task SetLikeAsync(Guid articleId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Unsets 'Like' mark at article
    /// </summary>
    Task UnsetLikeAsync(Guid articleId, Guid userId, CancellationToken cancellationToken);
}
