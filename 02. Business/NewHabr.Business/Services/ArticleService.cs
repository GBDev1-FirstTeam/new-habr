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

        ArgumentNullException.ThrowIfNull(articles, nameof(articles));

        var articlesDto = _mapper.Map<List<ArticleDto>>(articles);

        ArgumentNullException.ThrowIfNull(articlesDto, nameof(articlesDto));

        return articlesDto;
    }

    public async Task<IReadOnlyCollection<ArticleDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var articles = await _repositoryManager.ArticleRepository.GetByUserIdAsync(userId, cancellationToken);

        ArgumentNullException.ThrowIfNull(articles, nameof(articles));

        var articlesDto = _mapper.Map<List<ArticleDto>>(articles);

        ArgumentNullException.ThrowIfNull(articlesDto, nameof(articlesDto));

        return articlesDto;
    }

    public async Task<IReadOnlyCollection<ArticleDto>> GetPublishedAsync(CancellationToken cancellationToken = default)
    {
        var articles = await _repositoryManager.ArticleRepository.GetPublishedAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(articles, nameof(articles));

        var articlesDto = _mapper.Map<List<ArticleDto>>(articles);

        ArgumentNullException.ThrowIfNull(articlesDto, nameof(articlesDto));

        return articlesDto;
    }

    public async Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        var articles = await _repositoryManager.ArticleRepository.GetPublishedAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(articles, nameof(articles));

        var articlesDto = _mapper.Map<List<ArticleDto>>(articles);

        ArgumentNullException.ThrowIfNull(articlesDto, nameof(articlesDto));

        return articlesDto;
    }

    public async Task CreateAsync(CreateArticleRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var article = _mapper.Map<Article>(request);

        ArgumentNullException.ThrowIfNull(article, nameof(article));

        _repositoryManager.ArticleRepository.Create(article);
    }

    public Task UpdateAsync(ArticleDto updatedArticle, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
