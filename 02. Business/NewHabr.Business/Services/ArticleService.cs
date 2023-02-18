using AutoMapper;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace NewHabr.Business.Services;

public class ArticleService : IArticleService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public ArticleService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<CommentWithLikedMark>> GetCommentsWithLikedMarkAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdIncludeCommentLikesAsync(id, false, cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        var articleComments = article.Comments.OrderByDescending(c => c.CreatedAt);
        var comments = new List<CommentWithLikedMark>(article.Comments.Count);
        foreach (var comment in articleComments)
        {
            comments.Add(new CommentWithLikedMark
            {
                UserId = userId,
                Comment = _mapper.Map<CommentDto>(comment),
                IsLiked = comment.Likes.Any(l => l.UserId == userId),
            });
        }

        return comments;
    }

    public async Task<ArticleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdIncludeAsync(id, false, cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        return _mapper.Map<ArticleDto>(article);
    }

    public async Task<ArticlesGetResponse> GetUnpublishedAsync(
        ArticleQueryParameters queryParams,
        CancellationToken cancellationToken)
    {
        var articles = await _repositoryManager.ArticleRepository.GetUnpublishedIncludeAsync(queryParams, false, cancellationToken);
        return new ArticlesGetResponse
        {
            Metadata = articles.Metadata,
            Articles = _mapper.Map<List<ArticleDto>>(articles.ToList())
        };
    }

    public async Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        var articles = (await _repositoryManager.ArticleRepository.GetDeletedIncludeAsync(false, cancellationToken))
            .OrderByDescending(a => a.DeletedAt);
        return _mapper.Map<List<ArticleDto>>(articles);
    }

    public async Task CreateAsync(ArticleCreateRequest request, Guid creatorId, CancellationToken cancellationToken = default)
    {
        var creationDateTime = DateTimeOffset.UtcNow;
        var article = _mapper.Map<Article>(request);
        article.UserId = creatorId;
        article.CreatedAt = creationDateTime;
        article.ModifiedAt = creationDateTime;

        await UpdateCategoresAsync(article, request.Categories, cancellationToken);
        await UpdateTagsAsync(article, request.Tags, cancellationToken);

        _repositoryManager.ArticleRepository.Create(article);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task UpdateAsync(Guid articleId, Guid modifierId, ArticleUpdateRequest articleToUpdate, CancellationToken cancellationToken = default)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdIncludeAsync(articleId, trackChanges: true, cancellationToken);

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
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdIncludeAsync(id, true, cancellationToken);

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

    public async Task SetPublicationStatusAsync(Guid id, bool publicationStatus, CancellationToken cancellationToken = default)
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

        if (article.ApproveState != ApproveState.Approved && !article.Published)
        {
            throw new ArticleIsNotApproveException();
        }

        if (!article.Published)
        {
            article.Published = true;
            article.PublishedAt = DateTimeOffset.UtcNow;
        }
        else
        {
            article.Published = false;
            article.PublishedAt = null;
        }

        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task SetApproveStateAsync(Guid id, ApproveState state, CancellationToken cancellationToken = default)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdAsync(id, trackChanges: true, cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        article.ApproveState = state;
        await _repositoryManager.SaveAsync(cancellationToken);
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
                var newTag = _mapper.Map<Tag>(tagDto);
                article.Tags.Add(newTag);
            }
            else
            {
                article.Tags.Add(tag);
            }
        }
    }
}
