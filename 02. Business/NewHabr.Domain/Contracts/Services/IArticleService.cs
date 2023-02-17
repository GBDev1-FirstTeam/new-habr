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

    /// <exception cref="UserBannedException"></exception>
    /// <exception cref="CategoryNotFoundException"></exception>
    Task<ArticleDto> CreateAsync(Guid authorId, CreateArticleRequest request, CancellationToken cancellationToken = default);

    /// <exception cref="UserBannedException"></exception>
    /// <exception cref="ArticleNotFoundException"></exception>
    /// <exception cref="CategoryNotFoundException"></exception>
    /// <exception cref="InteractionOutsidePermissionException"></exception>
    Task UpdateAsync(Guid articleId, Guid authorId, UpdateArticleRequest articleToUpdate, CancellationToken cancellationToken = default);

    /// <exception cref="ArticleNotFoundException"></exception>
    /// <exception cref="InteractionOutsidePermissionException"></exception>
    Task DeleteByIdAsync(Guid articleId, Guid authorId, CancellationToken cancellationToken = default);

    /// <exception cref="ArticleNotFoundException"></exception>
    /// <exception cref="ArticleIsNotApprovedException"></exception>
    Task SetPublicationStatusAsync(Guid id, bool publicationStatus, CancellationToken cancellationToken = default);

    /// <exception cref="ArticleNotFoundException"></exception>
    Task SetApproveStateAsync(Guid id, ApproveState state, CancellationToken cancellationToken = default);
}
