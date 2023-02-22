using AutoMapper;
using NewHabr.Business.Extensions;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace NewHabr.Business.Services;

public class ArticleService : IArticleService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;

    public ArticleService(IRepositoryManager repositoryManager, IMapper mapper, INotificationService notificationService)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _notificationService = notificationService;
    }

    public async Task<IReadOnlyCollection<CommentWithLikedMark>> GetCommentsWithLikedMarkAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdIncludeCommentLikesAsync(id, false, cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        var comments = new List<CommentWithLikedMark>(article.Comments.Count);
        foreach (var comment in article.Comments)
        {
            comments.Add(new CommentWithLikedMark
            {
                UserId = userId,
                Comment = _mapper.Map<CommentDto>(comment),
                IsLiked = comment.Likes.Any(u => u.Id == userId),
            });
        }

        return comments;
    }

    public async Task<ArticleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdIncludeAsync(id, false, cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        return _mapper.Map<ArticleDto>(article);
    }

    public async Task<ArticlesGetResponse> GetPublishedAsync(
        ArticleQueryParametersDto queryParamsDto,
        CancellationToken cancellationToken)
    {
        var queryParams = _mapper.Map<ArticleQueryParameters>(queryParamsDto);
        var articles = await _repositoryManager.ArticleRepository.GetPublishedIncludeAsync(queryParams, false, cancellationToken);
        return new ArticlesGetResponse
        {
            Metadata = articles.Metadata,
            Articles = _mapper.Map<List<ArticleDto>>(articles.ToList())
        };
    }

    public async Task<ArticlesGetResponse> GetUnpublishedAsync(
        ArticleQueryParametersDto queryParamsDto,
        CancellationToken cancellationToken)
    {
        var queryParams = _mapper.Map<ArticleQueryParameters>(queryParamsDto);
        var articles = await _repositoryManager.ArticleRepository.GetUnpublishedIncludeAsync(queryParams, false, cancellationToken);
        return new ArticlesGetResponse
        {
            Metadata = articles.Metadata,
            Articles = _mapper.Map<List<ArticleDto>>(articles.ToList())
        };
    }

    public async Task<ArticlesGetResponse> GetDeletedAsync(
        ArticleQueryParametersDto queryParamsDto,
        CancellationToken cancellationToken)
    {
        var queryParams = _mapper.Map<ArticleQueryParameters>(queryParamsDto);
        var articles = await _repositoryManager.ArticleRepository.GetDeletedIncludeAsync(queryParams, false, cancellationToken);
        return new ArticlesGetResponse
        {
            Metadata = articles.Metadata,
            Articles = _mapper.Map<List<ArticleDto>>(articles.ToList())
        };
    }

    public async Task CreateAsync(ArticleCreateRequest request, Guid creatorId, CancellationToken cancellationToken)
    {
        await CheckIfUserNotBannedOrThrow(creatorId, cancellationToken);

        var creationDateTime = DateTimeOffset.UtcNow;
        var article = _mapper.Map<Article>(request);
        article.UserId = creatorId;
        article.CreatedAt = creationDateTime;
        article.ModifiedAt = creationDateTime;

        await UpdateCategoresAsync(article, request.Categories, cancellationToken);
        await UpdateTagsAsync(article, request.Tags, cancellationToken);

        _repositoryManager.ArticleRepository.Create(article);
        await _repositoryManager.SaveAsync(cancellationToken);
        await CreateNotificationIfMentionSomeoneAsync(article, cancellationToken);
    }

    public async Task UpdateAsync(Guid articleId, Guid modifierId, ArticleUpdateRequest articleToUpdate, CancellationToken cancellationToken)
    {
        await CheckIfUserNotBannedOrThrow(modifierId, cancellationToken);

        var article = await _repositoryManager
            .ArticleRepository
            .GetByIdWithTagsWithCategoriesAsync(articleId, trackChanges: true, cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        if (article.UserId != modifierId)
        {
            throw new UnauthorizedAccessException();
        }

        await UpdateCategoresAsync(article, articleToUpdate.Categories, cancellationToken);
        await UpdateTagsAsync(article, articleToUpdate.Tags, cancellationToken);

        _mapper.Map(articleToUpdate, article);
        article.ModifiedAt = DateTimeOffset.UtcNow;
        article.ApproveState = ApproveState.NotApproved;
        article.Published = false;
        article.PublishedAt = null;
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdAsync(id, true, cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        article.Deleted = true;
        article.DeletedAt = DateTimeOffset.UtcNow;
        article.Published = false;
        article.PublishedAt = null;
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task PublishAsync(Guid id, bool publicationStatus, CancellationToken cancellationToken)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdAsync(id, trackChanges: true, cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }
        else if (article.Published == publicationStatus)
        {
            return;
        }

        if (!article.Published)
        {
            if (article.ApproveState != ApproveState.Approved)
            {
                article.ApproveState = ApproveState.WaitApproval;
            }
            else if (article.ApproveState == ApproveState.Approved)
            {
                article.Published = true;
                article.PublishedAt = DateTimeOffset.UtcNow;
            }
        }
        else
        {
            article.Published = false;
            article.PublishedAt = null;
        }

        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task SetApproveStateAsync(Guid id, CancellationToken cancellationToken)
    {
        var article = await GetArticleAndCheckIfItExistsAsync(id, true, cancellationToken);

        if (article.ApproveState != ApproveState.WaitApproval)
        {
            return;
        }

        article.ApproveState = ApproveState.Approved;
        article.Published = true;
        article.PublishedAt = DateTimeOffset.UtcNow;
        await _repositoryManager.SaveAsync(cancellationToken);

        await CreateNotificationOnApprove(article, cancellationToken);
    }

    public async Task SetDisapproveStateAsync(Guid articleId, CancellationToken cancellationToken)
    {
        var article = await GetArticleAndCheckIfItExistsAsync(articleId, true, cancellationToken);

        if (article.ApproveState != ApproveState.WaitApproval)
        {
            return;
        }

        article.ApproveState = ApproveState.NotApproved;
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task SetLikeAsync(Guid articleId, Guid userId, CancellationToken cancellationToken)
    {
        var article = await _repositoryManager
            .ArticleRepository
            .GetArticleWithLikesAsync(articleId, true, cancellationToken);

        if (article is null)
            throw new ArticleNotFoundException();

        if (article.UserId == userId)
            return;

        await CheckIfUserNotBannedOrThrow(userId, cancellationToken);

        var user = await _repositoryManager.UserRepository.GetByIdAsync(userId, true, cancellationToken);

        if (!article.Likes.Any(u => u.Id == user!.Id))
        {
            article.Likes.Add(user);
            await _repositoryManager.SaveAsync(cancellationToken);
        }
    }

    public async Task UnsetLikeAsync(Guid articleId, Guid userId, CancellationToken cancellationToken)
    {
        var article = await _repositoryManager
            .ArticleRepository
            .GetArticleWithLikesAsync(articleId, true, cancellationToken);

        if (article is null)
            throw new ArticleNotFoundException();

        if (article.UserId == userId)
            return;

        var user = await _repositoryManager.UserRepository.GetByIdAsync(userId, true, cancellationToken);

        if (article.Likes.Remove(article.Likes.SingleOrDefault(u => u.Id == user!.Id)))
        {
            await _repositoryManager.SaveAsync(cancellationToken);
        }
    }



    private async Task CheckIfUserNotBannedOrThrow(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _repositoryManager.UserRepository.GetByIdAsync(userId, true, cancellationToken);

        if (user!.Banned)
            throw new UserBannedException(user.BannedAt!.Value);
    }

    private async Task<Article> GetArticleAndCheckIfItExistsAsync(Guid articleId, bool trackChanges, CancellationToken cancellationToken)
    {
        var article = await _repositoryManager
            .ArticleRepository
            .GetByIdAsync(articleId, trackChanges, cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        return article;
    }

    /// <summary>
    /// Update <paramref name="article"/> category list by clearing and then adding the receiving <paramref name="categoresDto"/>.
    /// </summary>
    /// <remarks>
    /// In proccess of adding <paramref name="categoresDto"/>, comparing them with existing in categores repository.
    /// If one of them doesn't contains in repository, throw exception.
    /// </remarks>
    /// <exception cref="CategoryNotFoundException"></exception>
    private async Task UpdateCategoresAsync(Article article, CategoryUpdateRequest[] categoresDto, CancellationToken cancellationToken)
    {
        if (article.Categories.Count != 0)
        {
            article.Categories.Clear();
        }

        var existsCategories = await _repositoryManager.CategoryRepository.GetAvaliableAsync(trackChanges: true, cancellationToken);

        foreach (var categoryDto in categoresDto)
        {
            var category = existsCategories.FirstOrDefault(c => c.Name == categoryDto.Name);

            if (category is null)
            {
                throw new CategoryNotFoundException();
            }

            article.Categories.Add(category);
        }
    }

    /// <summary>
    /// Update <paramref name="article"/> tag list by clearing and then adding the receiving <paramref name="tagsDto"/>.
    /// </summary>
    /// <remarks>
    /// In proccess of adding <paramref name="tagsDto"/>, comparing them with existing in tags repository.
    /// If one of them doesn't contains in repository, adding to both (Articles, Tags).
    /// </remarks>
    private async Task UpdateTagsAsync(Article article, TagCreateRequest[] tagsDto, CancellationToken cancellationToken)
    {
        if (article.Tags.Count != 0)
        {
            article.Tags.Clear();
        }

        tagsDto = tagsDto.DistinctBy(t => t.Name).ToArray();
        var existsTags = await _repositoryManager.TagRepository.GetAvaliableAsync(trackChanges: true, cancellationToken);

        foreach (var tagDto in tagsDto)
        {
            var tag = existsTags.FirstOrDefault(c => c.Name == tagDto.Name);

            if (tag is null)
            {
                tag = _mapper.Map<Tag>(tagDto);
            }
            article.Tags.Add(tag);
        }
    }

    private async Task CreateNotificationIfMentionSomeoneAsync(Article article, CancellationToken cancellationToken)
    {
        var usernames = article.Content.FindMentionedUsers();
        if (usernames.Count == 0)
            return;

        var users = await _repositoryManager
            .UserRepository
            .GetUsersByLoginAsync(usernames, true, cancellationToken);
        if (users.Count == 0)
            return;

        var notification = new NotificationCreateRequest
        {
            Text = $"Вас упомянули в статье '{article.Title}'"
        };
        await _notificationService
            .CreateAsync(notification, users, cancellationToken);
    }

    private async Task CreateNotificationOnApprove(Article article, CancellationToken cancellationToken)
    {
        var user = await _repositoryManager
            .UserRepository
            .GetByIdAsync(article.UserId, true, cancellationToken);

        if (user is null)
            throw new UserNotFoundException();

        var notification = new NotificationCreateRequest
        {
            Text = $"Ваша статья '{article.Title}' была согласована модератором"
        };

        await _notificationService
            .CreateAsync(notification, user, cancellationToken);
    }

}
