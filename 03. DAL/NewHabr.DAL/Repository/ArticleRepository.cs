using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.DAL.Extensions;
using NewHabr.Domain;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;
using NewHabr.Domain.ServiceModels;

namespace NewHabr.DAL.Repository;

public class ArticleRepository : RepositoryBase<Article, Guid>, IArticleRepository
{
    public ArticleRepository(ApplicationContext context) : base(context)
    {
    }


    public async Task<Article?> GetByIdAsync(Guid articleId, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetById(articleId, trackChanges)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Article?> GetByIdWithTagsWithCategoriesAsync(Guid articleId, bool trackChanges, CancellationToken cancellationToken)
    {
        return await FindByCondition(a => a.Id == articleId, trackChanges)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Article?> GetArticleWithLikesAsync(Guid articleId, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetById(articleId, trackChanges)
            .Include(a => a.Likes)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ArticleModel?> GetByIdAsync(Guid articleId, Guid whoAskingId, bool withComments, CancellationToken cancellationToken)
    {
        var query = FindByCondition(article => article.Id.Equals(articleId), false);
        return await GetOneAsync(query, whoAskingId, withComments, cancellationToken);
    }

    public async Task<PagedList<ArticleModel>> GetPublishedAsync(Guid whoAskingId, bool withComments, ArticleQueryParameters queryParams, CancellationToken cancellationToken)
    {
        var query = FindByCondition(article => article.Published, false)
            .Where(article => article.PublishedAt >= queryParams.From && article.PublishedAt <= queryParams.To)
            .OrderByType(article => article.PublishedAt, queryParams.OrderBy);
        return await GetManyAsync(query, whoAskingId, withComments, queryParams, cancellationToken);
    }

    public async Task<PagedList<ArticleModel>> GetByTitleAsync(string title, Guid whoAskingId, bool withComments, ArticleQueryParameters queryParams, CancellationToken cancellationToken)
    {
        var query = FindByCondition(article => article.Title.ToLower() == title.ToLower() && article.Published, false)
            .Where(article => article.PublishedAt >= queryParams.From && article.PublishedAt <= queryParams.To)
            .OrderByType(article => article.PublishedAt, queryParams.OrderBy);
        return await GetManyAsync(query, whoAskingId, withComments, queryParams, cancellationToken);
    }

    public async Task<PagedList<ArticleModel>> GetUnpublishedAsync(ArticleQueryParameters queryParams, CancellationToken cancellationToken)
    {
        var query = FindByCondition(a => !a.Published, false)
            .Where(article => article.CreatedAt >= queryParams.From && article.CreatedAt <= queryParams.To)
            .OrderByType(article => article.CreatedAt, queryParams.OrderBy);
        return await GetManyAsync(query, Guid.Empty, false, queryParams, cancellationToken);
    }

    public async Task<PagedList<ArticleModel>> GetDeletedAsync(ArticleQueryParameters queryParams, CancellationToken cancellationToken)
    {
        var query = GetDeleted(false)
            .Where(article => article.CreatedAt >= queryParams.From && article.CreatedAt <= queryParams.To)
            .OrderByType(article => article.CreatedAt, queryParams.OrderBy);
        return await GetManyAsync(query, Guid.Empty, false, queryParams, cancellationToken);
    }

    public async Task<PagedList<ArticleModel>> GetByAuthorIdAsync(Guid authorId, Guid whoAskingId, bool withComments, ArticleQueryParameters queryParams, CancellationToken cancellationToken)
    {
        var query = FindByCondition(article => article.UserId == authorId && article.Published, false)
            .Where(article => article.PublishedAt >= queryParams.From && article.PublishedAt <= queryParams.To)
            .OrderByType(article => article.PublishedAt, queryParams.OrderBy);
        return await GetManyAsync(query, whoAskingId, withComments, queryParams, cancellationToken);
    }

    public async Task<PagedList<ArticleModel>> GetUserLikedArticlesAsync(Guid userId, Guid whoAskingId, bool withComments, ArticleQueryParameters queryParams, CancellationToken cancellationToken)
    {
        var query = FindByCondition(article => article.Published && article.Likes.Any(u => u.Id == userId), false)
            .Where(article => article.PublishedAt >= queryParams.From && article.PublishedAt <= queryParams.To)
            .OrderByType(article => article.PublishedAt, queryParams.OrderBy);
        return await GetManyAsync(query, whoAskingId, withComments, queryParams, cancellationToken);
    }



    private async Task<ArticleModel?> GetOneAsync(IQueryable<Article> query, Guid whoAskingId, bool withComments, CancellationToken cancellationToken)
    {
        return await query
            .Select(ArticleToArticleModel(whoAskingId, withComments))
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<PagedList<ArticleModel>> GetManyAsync(IQueryable<Article> query, Guid whoAskingId, bool withComments, ArticleQueryParameters queryParams, CancellationToken cancellationToken)
    {
        var articleModels = query
            .Select(ArticleToArticleModel(whoAskingId, withComments));

        if (queryParams.ByRating == QueryParametersDefinitions.RatingOrderBy.Descending)
        {
            articleModels = articleModels
                .OrderByDescending(article => article.LikesCount);
        }
        else if (queryParams.ByRating == QueryParametersDefinitions.RatingOrderBy.Ascending)
        {
            articleModels = articleModels
                .OrderBy(article => article.LikesCount);
        }

        return await articleModels
            .ToPagedListAsync(queryParams.PageNumber, queryParams.PageSize, cancellationToken);
    }

    private static Expression<Func<Article, ArticleModel>> ArticleToArticleModel(Guid whoAskingId, bool includeComments)
    {
        return article => new ArticleModel
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            ImgURL = article.ImgURL,
            UserId = article.UserId,
            UserName = article.User.UserName,
            Categories = article.Categories,
            Tags = article.Tags,
            Comments = includeComments ? IncludeComments(whoAskingId, article) : Array.Empty<CommentModel>(),
            ApproveState = article.ApproveState,
            CreatedAt = article.CreatedAt,
            ModifiedAt = article.ModifiedAt,
            Published = article.Published,
            PublishedAt = article.PublishedAt,
            Deleted = article.Deleted,
            DeletedAt = article.DeletedAt,
            CommentsCount = article.Comments.Count(),
            LikesCount = article.Likes.Count(),
            IsLiked = Guid.Empty.Equals(whoAskingId) ? false : article.Likes.Any(sender => sender.Id.Equals(whoAskingId))
        };
    }

    private static CommentModel[] IncludeComments(Guid whoAskingId, Article article)
    {
        return article
            .Comments
            .OrderBy(comment => comment.CreatedAt)
            .Select(comment => new CommentModel
            {
                ArticleId = comment.ArticleId,
                CreatedAt = comment.CreatedAt,
                Id = comment.Id,
                ModifiedAt = comment.ModifiedAt,
                Text = comment.Text,
                UserId = comment.UserId,
                UserName = comment.User.UserName,
                LikesCount = comment.Likes.Count(),
                IsLiked = Guid.Empty.Equals(whoAskingId) ? false : comment.Likes.Any(sender => sender.Id.Equals(whoAskingId))
            })
            .ToArray();
    }
}
