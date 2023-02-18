using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IArticleService
{
    Task<IReadOnlyCollection<CommentWithLikedMark>> GetCommentsWithLikedMarkAsync(Guid id, Guid userId, CancellationToken cancellationToken);

    Task<ArticleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ArticlesGetResponse> GetUnpublishedAsync(ArticleQueryParameters queryParams, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default);

    Task CreateAsync(ArticleCreateRequest request, Guid creatorId, CancellationToken cancellationToken = default);

    Task UpdateAsync(Guid articleId, Guid modifierId, ArticleUpdateRequest articleToUpdate, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task SetPublicationStatusAsync(Guid id, bool publicationStatus, CancellationToken cancellationToken = default);

    Task SetApproveStateAsync(Guid id, ApproveState state, CancellationToken cancellationToken = default);
}
