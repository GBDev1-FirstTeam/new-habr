using NewHabr.DAL.EF;
using NewHabr.DAL.Repository.Impl;
using NewHabr.Domain.Contracts;

namespace NewHabr.DAL.Repository;
public class RepositoryManager : IRepositoryManager
{
    private readonly ApplicationContext _context;
    private readonly Lazy<ArticleRepository> _articleRepository;
    private readonly Lazy<CommentRepository> _commentRepository;
    private readonly Lazy<UserRepository> _userRepository;
    private readonly Lazy<CategoryRepository> _categoryRepository;
    private readonly Lazy<TagRepository> _tagRepository;

    public RepositoryManager(ApplicationContext context)
    {
        _context = context;
        _articleRepository = new Lazy<ArticleRepository>(() => new(_context));
        _commentRepository = new Lazy<CommentRepository>(() => new(_context));
        _userRepository = new Lazy<UserRepository>(() => new(_context));
        _categoryRepository = new Lazy<CategoryRepository>(() => new(_context));
        _tagRepository = new Lazy<TagRepository>(() => new(_context));
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
