﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        var article = await _repositoryManager.ArticleRepository.FindByCondition(a => a.Id == id && !a.Deleted)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments).ThenInclude(c => c.Likes)
            .FirstOrDefaultAsync(cancellationToken);

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
        var article = await _repositoryManager.ArticleRepository.GetById(id)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments)
            .FirstOrDefaultAsync(cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        return _mapper.Map<ArticleDto>(article);
    }
    public async Task<IReadOnlyCollection<ArticleDto>> GetUnpublishedAsync(CancellationToken cancellationToken = default)
    {
        var articles = (await _repositoryManager.ArticleRepository.FindByCondition(a => !a.Published && !a.Deleted)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments)
            .ToListAsync(cancellationToken)).OrderByDescending(a => a.CreatedAt);

        return _mapper.Map<List<ArticleDto>>(articles);
    }
    public async Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        var articles = (await _repositoryManager.ArticleRepository.GetDeleted()
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments)
            .ToListAsync(cancellationToken)).OrderByDescending(a => a.DeletedAt);

        return _mapper.Map<List<ArticleDto>>(articles);
    }
    public async Task<Guid> CreateAsync(CreateArticleRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _repositoryManager.UserRepository.GetById(request.UserId).FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var article = _mapper.Map<Article>(request);
        var creationDateTime = DateTimeOffset.UtcNow;
        article.CreatedAt = creationDateTime;
        article.ModifiedAt = creationDateTime;

        await UpdateCategoresAsync(article, request.Categories);
        await UpdateTagsAsync(article, request.Tags);

        var newArticleId = _repositoryManager.ArticleRepository.Create(article);
        await _repositoryManager.SaveAsync(cancellationToken);
        return newArticleId;
    }
    public async Task UpdateAsync(Guid id, UpdateArticleRequest articleToUpdate, CancellationToken cancellationToken = default)
    {
        var article = await _repositoryManager.ArticleRepository.GetById(id, trackChanges: true)
            .Include(a => a.Categories)
            .Include(a => a.Tags)
            .Include(a => a.Comments)
            .FirstOrDefaultAsync(cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        await UpdateCategoresAsync(article, articleToUpdate.Categories);
        await UpdateTagsAsync(article, articleToUpdate.Tags);

        _mapper.Map(articleToUpdate, article);
        article.ModifiedAt = DateTimeOffset.UtcNow;
        await _repositoryManager.SaveAsync(cancellationToken);
    }
    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await _repositoryManager.ArticleRepository.GetById(id, trackChanges: true)
            .FirstOrDefaultAsync(cancellationToken);

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
        var article = await _repositoryManager.ArticleRepository.GetById(id, trackChanges: true)
            .FirstOrDefaultAsync(cancellationToken);

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
        var article = await _repositoryManager.ArticleRepository.GetById(id, trackChanges: true)
            .FirstOrDefaultAsync(cancellationToken);

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
    private async Task UpdateCategoresAsync(Article article, CreateCategoryRequest[] categoriesDto)
    {
        if (article.Categories.Count != 0)
        {
            article.Categories.Clear();
        }

        foreach (var categoryDto in categoriesDto)
        {
            var existingCategory = await _repositoryManager.CategoryRepository.GetAvailable(trackChanges: true)
                .FirstOrDefaultAsync(c => c.Name == categoryDto.Name);

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
    private async Task UpdateTagsAsync(Article article, CreateTagRequest[] tagsDto)
    {
        if (article.Tags.Count != 0)
        {
            article.Tags.Clear();
        }

        tagsDto = tagsDto.DistinctBy(t => t.Name).ToArray();

        foreach (var tagDto in tagsDto)
        {
            var existingTag = await _repositoryManager.TagRepository.GetAvailable(trackChanges: true)
                .FirstOrDefaultAsync(c => c.Name == tagDto.Name);

            article.Tags.Add(existingTag ?? _mapper.Map<Tag>(tagDto));
        }
    }
}
