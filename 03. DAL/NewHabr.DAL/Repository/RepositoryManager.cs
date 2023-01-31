using NewHabr.DAL.EF;
using NewHabr.DAL.Repository.Impl;

namespace NewHabr.DAL.Repository;
public class RepositoryManager : IRepositoryManager
{
    private readonly ApplicationContext _context;
    public IArticleRepository ArticleRepository { get; }
    public ICommentRepository CommentRepository { get; }
    public IUserRepository UserRepository { get; }
    public RepositoryManager(ApplicationContext context)
    {
        _context = context;
        ArticleRepository = new ArticleRepository(_context);
        CommentRepository = new CommentRepository(_context);
        UserRepository = new UserRepository(_context);
    }
    
    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}


