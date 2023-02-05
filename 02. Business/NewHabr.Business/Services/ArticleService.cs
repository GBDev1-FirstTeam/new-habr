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
        var article = await _repositoryManager.ArticleRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        return _mapper.Map<ArticleDto>(article);
    }

    public async Task<IReadOnlyCollection<ArticleDto>> GetUnpublishedAsync(CancellationToken cancellationToken = default)
    {
        var articles = (await _repositoryManager.ArticleRepository.GetUnpublishedAsync(cancellationToken))
            .OrderByDescending(a => a.CreatedAt);
        return _mapper.Map<List<ArticleDto>>(articles);
    }

    public async Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        var articles = (await _repositoryManager.ArticleRepository.GetDeletedAsync(cancellationToken))
            .OrderByDescending(a => a.DeletedAt);
        return _mapper.Map<List<ArticleDto>>(articles);
    }

    public async Task CreateAsync(CreateArticleRequest request, CancellationToken cancellationToken = default)
    {
        var creationDateTime = DateTimeOffset.UtcNow;
        var article = _mapper.Map<Article>(request);
        article.CreatedAt = creationDateTime;
        article.ModifiedAt = creationDateTime;
        _repositoryManager.ArticleRepository.Create(article);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task UpdateAsync(UpdateArticleRequest articleToUpdate, CancellationToken cancellationToken = default)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdAsync(articleToUpdate.Id, cancellationToken: cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        article = _mapper.Map<Article>(articleToUpdate);
        article.ModifiedAt = DateTimeOffset.UtcNow;
        _repositoryManager.ArticleRepository.Update(article);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdAsync(id, true, cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        article.Deleted = true;
        article.DeletedAt = DateTimeOffset.UtcNow;
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task SetPublicationStatusAsync(SetArticlePublicationStatusRequest request, CancellationToken cancellationToken = default)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }
        else if (article.Published == request.PublicationStatus)
        {
            return;
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
}
