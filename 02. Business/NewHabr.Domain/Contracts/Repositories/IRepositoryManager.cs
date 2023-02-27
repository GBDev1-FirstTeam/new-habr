using NewHabr.Domain.Contracts.Repositories;

namespace NewHabr.Domain.Contracts;

public interface IRepositoryManager
{
    public IArticleRepository ArticleRepository { get; }
    public ICommentRepository CommentRepository { get; }
    public IUserRepository UserRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public ITagRepository TagRepository { get; }
    public ISecureQuestionsRepository SecureQuestionsRepository { get; }
    public INotificationRepository NotificationRepository { get; }

    public Task SaveAsync(CancellationToken cancellationToken);
}
