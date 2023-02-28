using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;
using NewHabr.Domain.ServiceModels;

namespace NewHabr.Domain.Contracts;

public interface IArticleRepository : IRepository<Article, Guid>
{
    Task<Article?> GetByIdAsync(Guid articleId, bool trackChanges, CancellationToken cancellationToken);

    Task<Article?> GetByIdWithTagsWithCategoriesAsync(Guid articleId, bool trackChanges, CancellationToken cancellationToken);

    Task<Article?> GetArticleWithLikesAsync(Guid articleId, bool trackChanges, CancellationToken cancellationToken);

    Task<ArticleModel?> GetByIdAsync(Guid articleId, Guid whoAskingId, bool withComments, CancellationToken cancellationToken);

    Task<PagedList<ArticleModel>> GetPublishedAsync(Guid whoAskingId, bool withComments, ArticleQueryParameters queryParams, CancellationToken cancellationToken);

    Task<PagedList<ArticleModel>> GetByTitleAsync(string title, Guid whoAskingId, bool withComments, ArticleQueryParameters queryParams, CancellationToken cancellationToken);

    Task<PagedList<ArticleModel>> GetUnpublishedAsync(ArticleQueryParameters queryParams, CancellationToken cancellationToken);

    Task<PagedList<ArticleModel>> GetDeletedAsync(ArticleQueryParameters queryParams, CancellationToken cancellationToken);

    /// <summary>
    /// Get published articles belonging to Author
    /// </summary>
    Task<PagedList<ArticleModel>> GetByAuthorIdAsync(Guid authorId, Guid whoAskingId, bool withComments, ArticleQueryParameters queryParams, CancellationToken cancellationToken);

    /// <summary>
    /// Get published and unpublished articles belonging to Author
    /// </summary>
    Task<PagedList<ArticleModel>> GetAllByAuthorIdAsync(Guid authorId, bool withComments, ArticleQueryParameters queryParams, CancellationToken cancellationToken);

    Task<PagedList<ArticleModel>> GetUserLikedArticlesAsync(Guid userId, Guid whoAskingId, bool withComments, ArticleQueryParameters queryParams, CancellationToken cancellationToken);
}
