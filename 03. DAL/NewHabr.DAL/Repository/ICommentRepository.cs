using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;
public interface ICommentRepository : IRepository<Comment, Guid>
{
    Task<IReadOnlyCollection<Comment>> GetByUserId(Guid userId);
    Task<IReadOnlyCollection<Comment>> GetByArticleId(Guid articleId);
    Task<IReadOnlyCollection<Comment>> GetByCreationTime(DateTimeOffset creationTime);
}
