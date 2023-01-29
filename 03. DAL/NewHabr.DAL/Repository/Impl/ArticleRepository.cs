using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewHabr.DAL.EF;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository.Impl;
public class ArticleRepository : IArticleRepository
{
    private readonly ILogger<ArticleRepository> _logger;
    private readonly ApplicationContext _context;

    public ArticleRepository(ILogger<ArticleRepository> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Guid> Create(Article data)
    {
        await _context.Articles.AddAsync(data);
        await _context.SaveChangesAsync();
        return data.Id;
    }

    public async Task<int> Delete(Guid id)
    {
        Article article = await _context.Articles.FirstOrDefaultAsync(ar => ar.Id == id);
        if (article == null)
        {
            _logger.LogInformation($"Article {id} not found");
            return 0;
        }
        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();
        return 1;
    }

    public async Task<IReadOnlyCollection<Article>> GetAll()
    {
        return await _context.Articles.ToListAsync();
    }

    public async Task<IReadOnlyCollection<Article>> GetByCreatedTime(DateTime createdTime)
    {
        return await _context.Articles.Where(ar => ar.CreatedAt == createdTime).ToListAsync();
    }

    public async Task<Article> GetById(Guid id)
    {
        Article article = await _context.Articles.FirstOrDefaultAsync(ar => ar.Id == id);
        if (article == null) 
        {
            _logger.LogInformation($"Article {id} not found");
            throw new ArgumentNullException(nameof(article));
        }
        return article;
    }

    public async Task<IReadOnlyCollection<Article>> GetByModifiedTime(DateTime modifiedTime)
    {
        return await _context.Articles.Where(ar => ar.ModifiedAt == modifiedTime).ToListAsync();
    }

    public async Task<IReadOnlyCollection<Article>> GetByPublishedTime(DateTime publishedTime)
    {
        return await _context.Articles.Where(ar => ar.PublishedAt == publishedTime).ToListAsync();
    }

    public async Task<Article> GetByTitle(string title)
    {
        Article article = await _context.Articles.FirstOrDefaultAsync(ar => ar.Title == title);
        if (article == null)
        {
            _logger.LogInformation($"Article with title \"{title}\" not found");
            throw new ArgumentNullException(nameof(article));
        }
        return article;
    }

    public async Task<IReadOnlyCollection<Article>> GetByUserId(Guid userId)
    {
        return await _context.Articles.Where(ar => ar.UserId == userId).ToListAsync();
    }

    public async Task<IReadOnlyCollection<Article>> GetDeleted()
    {
        return await _context.Articles.Where(ar => ar.Deleted == true).ToListAsync();
    }

    public async Task<IReadOnlyCollection<Article>> GetPublished()
    {
        return await _context.Articles.Where(ar => ar.Published == true).ToListAsync();
    }

    public async Task<int> Update(Article data)
    {
        Article article = await _context.Articles.FirstOrDefaultAsync(ar => ar.Id == data.Id);
        if (article == null)
        {
            _logger.LogInformation($"Article {data.Id} not found");
            return 0;
        }

        article.ApproveState = data.ApproveState;
        article.CreatedAt = data.CreatedAt;
        article.Content = data.Content;
        article.Categories = data.Categories;
        article.Comments = data.Comments;
        article.User = data.User;
        article.UserId = data.UserId;
        article.Deleted = data.Deleted;
        article.DeletedAt = data.DeletedAt;
        article.Published = data.Published;
        article.PublishedAt = data.PublishedAt;
        article.Likes = data.Likes;
        article.Tags = data.Tags;

        _context.Articles.Update(article);
        await _context.SaveChangesAsync();
        return 1;
    }
}
