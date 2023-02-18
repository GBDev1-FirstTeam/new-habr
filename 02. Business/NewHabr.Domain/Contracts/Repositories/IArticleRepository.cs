using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IArticleRepository : IRepository<Article, Guid>
{
    Task<IReadOnlyCollection<Article>> GetByTitleIncludeAsync(string title, bool trackChanges, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Article>> GetByUserIdIncludeAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);

    Task<PagedList<Article>> GetUnpublishedIncludeAsync(ArticleQueryParameters queryParams, bool trackChanges, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Article>> GetDeletedIncludeAsync(bool trackChanges, CancellationToken cancellationToken);

    Task<Article?> GetByIdIncludeAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);

    Task<Article?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);

    Task<Article?> GetByIdIncludeCommentLikesAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);

    Task<ICollection<UserArticle>> GetUserArticlesAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);

    Task<ICollection<UserLikedArticle>> GetUserLikedArticlesAsync(Guid userId, CancellationToken cancellationToken);

    Task<int> GetUnpublishedPageCountAsync(int pageSize, CancellationToken cancellationToken);
}
