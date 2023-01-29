using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;
public interface IArticleRepository : IRepository<Article, Guid>
{
    Task<Article> GetByTitle(string title);
    Task<IReadOnlyCollection<Article>> GetByUserId(Guid userId);
    Task<IReadOnlyCollection<Article>> GetByCreatedTime(DateTime createdTime);
    Task<IReadOnlyCollection<Article>> GetByModifiedTime(DateTime modifiedTime);
    Task<IReadOnlyCollection<Article>> GetByPublishedTime(DateTime publishedTime);
    Task<IReadOnlyCollection<Article>> GetPublished();
    Task<IReadOnlyCollection<Article>> GetDeleted();
}
