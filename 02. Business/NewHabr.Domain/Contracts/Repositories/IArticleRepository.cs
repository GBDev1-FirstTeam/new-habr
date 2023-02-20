using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IArticleRepository : IRepository<Article, Guid>
{
    Task<PagedList<ArticleExt>> GetPublishedIncludeAsync(
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken);

    Task<PagedList<ArticleExt>> GetByTitleIncludeAsync(
        string title,
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken);

    Task<PagedList<ArticleExt>> GetByUserIdIncludeAsync(
        Guid userId,
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken);

    Task<PagedList<ArticleExt>> GetUnpublishedIncludeAsync(
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken);

    Task<PagedList<ArticleExt>> GetDeletedIncludeAsync(
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken);

    Task<ArticleExt?> GetByIdIncludeAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);

    Task<Article?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);

    Task<ArticleExt?> GetByIdIncludeCommentLikesAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);

    Task<ICollection<UserArticle>> GetUserArticlesAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);

    Task<ICollection<UserLikedArticle>> GetUserLikedArticlesAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);

    Task<Article?> GetArticleWithLikesAsync(Guid articleId, bool trackChanges, CancellationToken cancellationToken);
}
