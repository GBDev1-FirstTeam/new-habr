using NewHabr.DAL.EF;
using NewHabr.DAL.Repository.Impl;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Repositories;

namespace NewHabr.DAL.Repository;
public class RepositoryManager : IRepositoryManager
{
    private readonly ApplicationContext _context;
    public IArticleRepository ArticleRepository { get; }
    public ICommentRepository CommentRepository { get; }
    public IUserRepository UserRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public ITagRepository TagRepository { get; }
    public ISecureQuestionsRepository SecureQuestionsRepository { get; }
    public INotificationRepository NotificationRepository { get; }

    public RepositoryManager(ApplicationContext context)
    {
        _context = context;
        ArticleRepository = new ArticleRepository(_context);
        CommentRepository = new CommentRepository(_context);
        UserRepository = new UserRepository(_context);
        CategoryRepository = new CategoryRepository(_context);
        TagRepository = new TagRepository(_context);
        SecureQuestionsRepository = new SecureQuestionsRepository(_context);
        NotificationRepository = new NotificationRepository(_context);
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
