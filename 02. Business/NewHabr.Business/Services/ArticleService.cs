using AutoMapper;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
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

    public async Task<IReadOnlyCollection<ArticleDto>> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentException(title, nameof(title));
        }

        var articles = await _repositoryManager.ArticleRepository.GetByTitleAsync(title, cancellationToken);

        if (articles is null)
        {
            return new List<ArticleDto>();
        }

        var articlesDto = _mapper.Map<List<ArticleDto>>(articles);

        return articlesDto ?? new List<ArticleDto>();
    }

    public async Task<IReadOnlyCollection<ArticleDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var articles = await _repositoryManager.ArticleRepository.GetByUserIdAsync(userId, cancellationToken);

        if (articles is null)
        {
            return new List<ArticleDto>();
        }

        var articlesDto = _mapper.Map<List<ArticleDto>>(articles);

        return articlesDto ?? new List<ArticleDto>();
    }

    public async Task<IReadOnlyCollection<ArticleDto>> GetPublishedAsync(CancellationToken cancellationToken = default)
    {
        var articles = await _repositoryManager.ArticleRepository.GetPublishedAsync(cancellationToken);

        if (articles is null)
        {
            return new List<ArticleDto>();
        }

        var articlesDto = _mapper.Map<List<ArticleDto>>(articles);

        return articlesDto ?? new List<ArticleDto>();
    }

    public async Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        var articles = await _repositoryManager.ArticleRepository.GetDeletedAsync(cancellationToken);

        if (articles is null)
        {
            return new List<ArticleDto>();
        }

        var articlesDto = _mapper.Map<List<ArticleDto>>(articles);

        return articlesDto ?? new List<ArticleDto>();
    }

    public async Task CreateAsync(CreateArticleRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var article = _mapper.Map<Article>(request);

        if (article is null)
        {
            throw new AutoMapperMappingException();
        }

        _repositoryManager.ArticleRepository.Create(article);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task UpdateAsync(ArticleDto updatedArticle, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(updatedArticle, nameof(updatedArticle));

        var article = _mapper.Map<Article>(updatedArticle);

        if (article is null)
        {
            throw new AutoMapperMappingException();
        }

        _repositoryManager.ArticleRepository.Update(article);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _repositoryManager.ArticleRepository.DeleteByIdAsync(id, cancellationToken);
        await _repositoryManager.SaveAsync(cancellationToken);
    }
}
