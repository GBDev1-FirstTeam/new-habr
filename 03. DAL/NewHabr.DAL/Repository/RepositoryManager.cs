using NewHabr.DAL.EF;
using NewHabr.DAL.Repository.Impl;
using NewHabr.Domain.Contracts;

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

    public RepositoryManager(ApplicationContext context)
    {
        _context = context;
        ArticleRepository = new ArticleRepository(_context);
        CommentRepository = new CommentRepository(_context);
        UserRepository = new UserRepository(_context);
        CategoryRepository = new CategoryRepository(_context);
        TagRepository = new TagRepository(_context);
        SecureQuestionsRepository = new SecureQuestionsRepository(_context);
    }

    public IArticleRepository ArticleRepository => _articleRepository.Value;
    public ICommentRepository CommentRepository => _commentRepository.Value;
    public IUserRepository UserRepository => _userRepository.Value;
    public ICategoryRepository CategoryRepository => _categoryRepository.Value;
    public ITagRepository TagRepository => _tagRepository.Value;

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
