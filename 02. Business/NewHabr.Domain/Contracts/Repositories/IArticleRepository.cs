using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IArticleRepository : IRepository<Article, Guid>
{
    Task<PagedList<Article>> GetPublishedIncludeAsync(
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken);

    Task<PagedList<Article>> GetByTitleIncludeAsync(
        string title,
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken);

    Task<PagedList<Article>> GetByUserIdIncludeAsync(
        Guid userId,
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken);

    Task<PagedList<Article>> GetUnpublishedIncludeAsync(
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken);

    Task<PagedList<Article>> GetDeletedIncludeAsync(
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken);

    Task<Article?> GetByIdIncludeAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);

    Task<Article?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);

    Task<Article?> GetByIdIncludeCommentLikesAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);

    Task<ICollection<UserArticle>> GetUserArticlesAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);

    Task<ICollection<UserLikedArticle>> GetUserLikedArticlesAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);

    Task<Article?> GetArticleWithLikesAsync(Guid articleId, bool trackChanges, CancellationToken cancellationToken);
}
