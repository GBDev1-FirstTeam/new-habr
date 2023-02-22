using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IArticleService
{
    Task<IReadOnlyCollection<CommentWithLikedMark>> GetCommentsWithLikedMarkAsync(Guid id, Guid userId, CancellationToken cancellationToken);

    Task<ArticleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ArticlesGetResponse> GetPublishedAsync(ArticleQueryParametersDto queryParams, CancellationToken cancellationToken);

    Task<ArticlesGetResponse> GetUnpublishedAsync(ArticleQueryParametersDto queryParams, CancellationToken cancellationToken);

    Task<ArticlesGetResponse> GetDeletedAsync(ArticleQueryParametersDto queryParams, CancellationToken cancellationToken);

    Task CreateAsync(ArticleCreateRequest request, Guid creatorId, CancellationToken cancellationToken);

    Task UpdateAsync(Guid articleId, Guid modifierId, ArticleUpdateRequest articleToUpdate, CancellationToken cancellationToken);

    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Switch Published state(published or unpublished). If Artice is not approved, approval process starts
    /// </summary>
    Task PublishAsync(Guid id, bool publicationStatus, CancellationToken cancellationToken);

    /// <summary>
    /// Approve article if in WaitApproval state, after approval, article turns published
    /// </summary>
    Task SetApproveStateAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Disapprove article if in WaitApproval state
    /// </summary>
    Task SetDisapproveStateAsync(Guid articleId, CancellationToken cancellationToken);

    /// <summary>
    /// Sets 'Like' mark at article
    /// </summary>
    Task SetLikeAsync(Guid articleId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Unsets 'Like' mark at article
    /// </summary>
    Task UnsetLikeAsync(Guid articleId, Guid userId, CancellationToken cancellationToken);
}
