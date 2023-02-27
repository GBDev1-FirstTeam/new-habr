using System;
using NewHabr.DAL.EF;
using NewHabr.DAL.Repository.Impl;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Repositories;

namespace NewHabr.DAL.Repository;
public class RepositoryManager : IRepositoryManager
{
    private readonly ApplicationContext _context;
    private readonly Lazy<IArticleRepository> _articleRepository;
    private readonly Lazy<ICommentRepository> _commentRepository;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<ICategoryRepository> _categoryRepository;
    private readonly Lazy<ITagRepository> _tagRepository;
    private readonly Lazy<ISecureQuestionsRepository> _secureQuestionsRepository;
    private readonly Lazy<INotificationRepository> _notificationRepository;


    public RepositoryManager(ApplicationContext context)
    {
        _context = context;
        _articleRepository = new Lazy<IArticleRepository>(() => new ArticleRepository(_context));
        _commentRepository = new Lazy<ICommentRepository>(() => new CommentRepository(_context));
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(_context));
        _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(_context));
        _tagRepository = new Lazy<ITagRepository>(() => new TagRepository(_context));
        _secureQuestionsRepository = new Lazy<ISecureQuestionsRepository>(() => new SecureQuestionsRepository(_context));
        _notificationRepository = new Lazy<INotificationRepository>(() => new NotificationRepository(_context));
    }


    public IArticleRepository ArticleRepository => _articleRepository.Value;
    public ICommentRepository CommentRepository => _commentRepository.Value;
    public IUserRepository UserRepository => _userRepository.Value;
    public ICategoryRepository CategoryRepository => _categoryRepository.Value;
    public ITagRepository TagRepository => _tagRepository.Value;
    public ISecureQuestionsRepository SecureQuestionsRepository => _secureQuestionsRepository.Value;
    public INotificationRepository NotificationRepository => _notificationRepository.Value;

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
