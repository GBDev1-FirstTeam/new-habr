using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewHabr.DAL.EF;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository.Impl;
public class CommentRepository : ICommentRepository
{
    private readonly ILogger<CommentRepository> _logger;
    private readonly ApplicationContext _context;

    public CommentRepository(ILogger<CommentRepository> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Guid> Create(Comment data)
    {
        await _context.Comments.AddAsync(data);
        await _context.SaveChangesAsync();
        return data.Id;
    }

    public async Task<int> Delete(Guid id)
    {
        Comment comment = await _context.Comments.FirstOrDefaultAsync(com => com.Id == id);
        if (comment is null)
        {
            _logger.LogInformation($"Comment {id} not found");
            return 0;
        }
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return 1;
    }

    public async Task<IReadOnlyCollection<Comment>> GetAll()
    {        
        return await _context.Comments.ToListAsync();
    }

    public async Task<IReadOnlyCollection<Comment>> GetByArticleId(Guid articleId)
    {        
        return await _context.Comments.Where(com => com.ArticleId == articleId).ToListAsync(); ;
    }

    public async Task<IReadOnlyCollection<Comment>> GetByCreationTime(DateTimeOffset creationTime)
    {        
        return await _context.Comments.Where(com => com.CreatedAt == creationTime).ToListAsync();
    }

    public async Task<Comment> GetById(Guid id)
    {
        Comment comment = await _context.Comments.FirstOrDefaultAsync(com => com.Id == id);
        if (comment is null)
        {
            _logger.LogInformation($"Comment {id} not found");
            throw new ArgumentNullException(nameof(comment));
        }
        
        return comment;
    }

    public async Task<IReadOnlyCollection<Comment>> GetByUserId(Guid userId)
    {        
        return await _context.Comments.Where(com => com.UserId == userId).ToListAsync();
    }

    public async Task<int> Update(Comment data)
    {
        Comment comment = await _context.Comments.FirstOrDefaultAsync(com => com.Id == data.Id);
        if (comment is null)
        {
            _logger.LogInformation($"Comment {data.Id} not found");
            return 0;
        }
        comment.Text = data.Text;
        comment.CreatedAt = data.CreatedAt;
        comment.User = data.User;
        comment.UserId = data.UserId;
        comment.Article = data.Article;
        comment.ArticleId = data.ArticleId;

        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
        return 1;
    }
}
