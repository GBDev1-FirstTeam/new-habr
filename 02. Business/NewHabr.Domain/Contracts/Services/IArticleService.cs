using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IArticleService
{
    Task<IReadOnlyCollection<CommentWithLikedMark>> GetCommentsWithLikedMarkAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken);

    Task<ArticleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ArticlesGetResponse> GetPublishedAsync(ArticleQueryParameters queryParams, CancellationToken cancellationToken);

    Task<ArticlesGetResponse> GetUnpublishedAsync(ArticleQueryParameters queryParams, CancellationToken cancellationToken);

    Task<ArticlesGetResponse> GetDeletedAsync(ArticleQueryParameters queryParams, CancellationToken cancellationToken);

    Task CreateAsync(ArticleCreateRequest request, Guid creatorId, CancellationToken cancellationToken);

    Task UpdateAsync(Guid articleId, Guid modifierId, ArticleUpdateRequest articleToUpdate, CancellationToken cancellationToken);

    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);

    Task SetPublicationStatusAsync(Guid id, bool publicationStatus, CancellationToken cancellationToken);

    Task SetApproveStateAsync(Guid id, ApproveState state, CancellationToken cancellationToken);
}
