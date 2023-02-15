using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IArticleRepository : IRepository<Article, Guid>
{
    Task<IReadOnlyCollection<Article>> GetByTitleIncludeAsync(
        string title,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Article>> GetByUserIdIncludeAsync(
        Guid userId,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Article>> GetUnpublishedIncludeAsync(
        bool trackChanges = false,
        CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Article>> GetPublishedIncludeAsync(
        int count,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Article>> GetDeletedIncludeAsync(
        bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<Article?> GetByIdIncludeAsync(
        Guid id,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<Article?> GetByIdAsync(
        Guid id,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<Article?> GetByIdIncludeCommentLikesAsync(
        Guid id,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<ICollection<UserArticle>> GetUserArticlesAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);

    Task<ICollection<UserLikedArticle>> GetUserLikedArticlesAsync(Guid userId, CancellationToken cancellationToken);
}
