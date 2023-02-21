using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.DAL.Extensions;
using NewHabr.Domain;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public class ArticleRepository : RepositoryBase<Article, Guid>, IArticleRepository
{
    public ArticleRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<PagedList<ArticleExt>> GetPublishedIncludeAsync(
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        return await FindByCondition(article => article.Published, trackChanges)
            .Where(a => a.CreatedAt >= queryParams.From && a.CreatedAt <= queryParams.To)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments
                .OrderBy(c => c.CreatedAt))
            .OrderByType(a => a.PublishedAt, queryParams.OrderBy)
            .Select(ArticleToArticleExt())
            .ToPagedListAsync(queryParams.PageNumber, queryParams.PageSize, cancellationToken);
    }

    public async Task<PagedList<ArticleExt>> GetByTitleIncludeAsync(
        string title,
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        return await FindByCondition(a => a.Title.ToLower() == title.ToLower(), trackChanges)
            .Where(a => a.CreatedAt >= queryParams.From && a.CreatedAt <= queryParams.To)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments
                .OrderBy(c => c.CreatedAt))
            .OrderByType(a => a.CreatedAt, queryParams.OrderBy)
            .Select(ArticleToArticleExt())
            .ToPagedListAsync(queryParams.PageNumber, queryParams.PageSize, cancellationToken);
    }

    public async Task<PagedList<ArticleExt>> GetByUserIdIncludeAsync(
        Guid userId,
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        return await FindByCondition(a => a.UserId == userId, trackChanges)
            .Where(a => a.CreatedAt >= queryParams.From && a.CreatedAt <= queryParams.To)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments
                .OrderBy(c => c.CreatedAt))
            .OrderByType(a => a.CreatedAt, queryParams.OrderBy)
            .Select(ArticleToArticleExt())
            .ToPagedListAsync(queryParams.PageNumber, queryParams.PageSize, cancellationToken);
    }

    public async Task<PagedList<ArticleExt>> GetUnpublishedIncludeAsync(
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        return await FindByCondition(a => !a.Published, trackChanges)
            .Where(a => a.CreatedAt >= queryParams.From && a.CreatedAt <= queryParams.To)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments
                .OrderBy(c => c.CreatedAt))
            .OrderByType(a => a.CreatedAt, queryParams.OrderBy)
            .Select(ArticleToArticleExt())
            .ToPagedListAsync(queryParams.PageNumber, queryParams.PageSize, cancellationToken);
    }

    public async Task<PagedList<ArticleExt>> GetDeletedIncludeAsync(
        ArticleQueryParameters queryParams,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        return await GetDeleted(trackChanges)
            .Where(a => a.CreatedAt >= queryParams.From && a.CreatedAt <= queryParams.To)
            .IgnoreQueryFilters()
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments
                .OrderBy(c => c.CreatedAt))
            .OrderByType(a => a.DeletedAt, queryParams.OrderBy)
            .Select(ArticleToArticleExt())
            .ToPagedListAsync(queryParams.PageNumber, queryParams.PageSize, cancellationToken);
    }

    public async Task<Article?> GetByIdAsync(
        Guid id,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        return await GetById(id, trackChanges)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ArticleExt?> GetByIdIncludeAsync(
        Guid id,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        return await FindByCondition(a => a.Id == id, trackChanges)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments)
            .Select(ArticleToArticleExt())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Article?> GetByIdWithTagsWithCategoriesAsync(
        Guid articleId,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        return await FindByCondition(a => a.Id == articleId, trackChanges)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ArticleExt?> GetByIdIncludeCommentLikesAsync(
        Guid id,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        return await FindByCondition(a => a.Id == id, trackChanges)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments
                .OrderBy(c => c.CreatedAt))
                .ThenInclude(c => c.Likes)
            .Select(ArticleToArticleExt())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ICollection<UserArticle>> GetUserArticlesAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        return await FindByCondition(article => article.UserId == userId, trackChanges)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Select(row => new UserArticle
            {
                Id = row.Id,
                Title = row.Title,
                Content = row.Content,
                ImgURL = row.ImgURL,
                Categories = row.Categories,
                Tags = row.Tags,
                CommentsCount = row.Comments.Count,
                LikesCount = row.Likes.Count,
                CreatedAt = row.CreatedAt,
                ModifiedAt = row.ModifiedAt,
                Published = row.Published,
                PublishedAt = row.PublishedAt.HasValue ? row.PublishedAt.Value : default(DateTimeOffset),
                ApproveState = row.ApproveState
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<UserLikedArticle>> GetUserLikedArticlesAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        return await FindByCondition(article => article.Published, trackChanges)
            .Include(article => article.Tags)
            .Include(article => article.Categories)
            .Include(article => article.Likes)
            .Where(a => a.Likes.Any(u => u.Id == userId))
            .Select(row => new UserLikedArticle
            {
                Id = row.Id,
                Title = row.Title,
                Categories = row.Categories,
                Tags = row.Tags
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<Article?> GetArticleWithLikesAsync(Guid articleId, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetById(articleId, trackChanges)
            .Include(a => a.Likes)
            .FirstOrDefaultAsync(cancellationToken);
    }



    private static Expression<Func<Article, ArticleExt>> ArticleToArticleExt()
    {
        return article => new ArticleExt
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            Categories = article.Categories,
            Tags = article.Tags,
            Comments = article.Comments.Select(comment => new CommentExt
            {
                ArticleId = comment.ArticleId,
                CreatedAt = comment.CreatedAt,
                Id = comment.Id,
                ModifiedAt = comment.ModifiedAt,
                Text = comment.Text,
                UserId = comment.UserId,
                UserName = comment.User.UserName,
                Likes = comment.Likes
            }).ToArray(),
            ApproveState = article.ApproveState,
            CreatedAt = article.CreatedAt,
            ImgURL = article.ImgURL,
            ModifiedAt = article.ModifiedAt,
            Published = article.Published,
            PublishedAt = article.PublishedAt,
            UserName = article.User.UserName,
            UserId = article.UserId,
            Deleted = article.Deleted,
            DeletedAt = article.DeletedAt,
            Likes = article.Likes
        };
    }
}
