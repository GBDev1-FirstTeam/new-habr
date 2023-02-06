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

    public async Task<ArticleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdIncludeAsync(id, cancellationToken: cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        return _mapper.Map<ArticleDto>(article);
    }
    public async Task<IReadOnlyCollection<ArticleDto>> GetUnpublishedAsync(CancellationToken cancellationToken = default)
    {
        var articles = (await _repositoryManager.ArticleRepository.GetUnpublishedIncludeAsync(cancellationToken: cancellationToken))
            .OrderByDescending(a => a.CreatedAt);
        return _mapper.Map<List<ArticleDto>>(articles);
    }
    public async Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        var articles = (await _repositoryManager.ArticleRepository.GetDeletedIncludeAsync(cancellationToken: cancellationToken))
            .OrderByDescending(a => a.DeletedAt);
        return _mapper.Map<List<ArticleDto>>(articles);
    }
    public async Task CreateAsync(CreateArticleRequest request, CancellationToken cancellationToken = default)
    {
        var creationDateTime = DateTimeOffset.UtcNow;
        var article = _mapper.Map<Article>(request);
        article.CreatedAt = creationDateTime;
        article.ModifiedAt = creationDateTime;

        UpdateCategores(article, request.Categories);
        UpdateTags(article, request.Tags);

        foreach (var tagDto in request.Tags)
        {
            var existingTag = _repositoryManager.TagRepository.FindAll(true)
                .FirstOrDefault(c => c.Name == tagDto.Name && !c.Deleted);

            article.Tags.Add(existingTag ?? _mapper.Map<Tag>(tagDto));
        }

        _repositoryManager.ArticleRepository.Create(article);
        await _repositoryManager.SaveAsync(cancellationToken);
    }
    public async Task UpdateAsync(UpdateArticleRequest articleToUpdate, CancellationToken cancellationToken = default)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdIncludeAsync(articleToUpdate.Id, trackChanges: true, cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        UpdateCategores(article, articleToUpdate.Categories);
        UpdateTags(article, articleToUpdate.Tags);

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
        var article = await _repositoryManager.ArticleRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }
        else if (article.Published == publicationStatus)
        {
            return;
        }

        if(article.ApproveState != ApproveState.Approved)
        {
            throw new ArticleIsNotApproveException();
        }

        var publishedDateTime = DateTimeOffset.UtcNow;

        if (!article.Published)
        {
            article.Published = true;
            article.PublishedAt = publishedDateTime;
        }
        else
        {
            article.Published = false;
            article.PublishedAt = null;
        }

        _repositoryManager.ArticleRepository.Update(article);
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
    private void UpdateCategores(Article article, CreateCategoryRequest[] categoresDto)
    {
        if (article.Categories.Count != 0)
        {
            article.Categories.Clear();
        }

        foreach (var categoryDto in categoresDto)
        {
            var existingCategory = _repositoryManager.CategoryRepository.FindAll(trackChanges: true)
                .FirstOrDefault(c => c.Name == categoryDto.Name && !c.Deleted);

            article.Categories.Add(existingCategory ?? throw new CategoryNotFoundException());
        }
    }

    /// <summary>
    /// Update <paramref name="article"/> tag list by clearing and then adding the receiving <paramref name="tagsDto"/>.
    /// </summary>
    /// <remarks>
    /// In proccess of adding <paramref name="tagsDto"/>, comparing them with existing in tags repository.
    /// If one of them doesn't contains in repository, adding to both (Articles, Tags).
    /// </remarks>
    private void UpdateTags(Article article, CreateTagRequest[] tagsDto)
    {
        if(article.Tags.Count != 0)
        {
            article.Tags.Clear();
        }

        foreach (var tagDto in tagsDto)
        {
            var existingTag = _repositoryManager.TagRepository.FindAll(trackChanges: true)
                .FirstOrDefault(c => c.Name == tagDto.Name && !c.Deleted);

            article.Tags.Add(existingTag ?? _mapper.Map<Tag>(tagDto));
        }
    }
}
