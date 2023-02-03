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
        var articles = await _repositoryManager.ArticleRepository.GetByTitleAsync(title, cancellationToken);
        var articlesDto = _mapper.Map<List<ArticleDto>>(articles);
        return articlesDto;
    }

    public async Task<IReadOnlyCollection<ArticleDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var articles = await _repositoryManager.ArticleRepository.GetByUserIdAsync(userId, cancellationToken);
        var articlesDto = _mapper.Map<List<ArticleDto>>(articles);
        return articlesDto;
    }

    public async Task<IReadOnlyCollection<ArticleDto>> GetPublishedAsync(CancellationToken cancellationToken = default)
    {
        var articles = await _repositoryManager.ArticleRepository.GetPublishedAsync(cancellationToken);
        var articlesDto = _mapper.Map<List<ArticleDto>>(articles);
        return articlesDto;
    }

    public async Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        var articles = await _repositoryManager.ArticleRepository.GetDeletedAsync(cancellationToken);
        var articlesDto = _mapper.Map<List<ArticleDto>>(articles);
        return articlesDto;
    }

    public async Task CreateAsync(CreateArticleRequest request, CancellationToken cancellationToken = default)
    {
        var article = _mapper.Map<Article>(request);
        _repositoryManager.ArticleRepository.Create(article);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task UpdateAsync(ArticleDto articleToUpdate, CancellationToken cancellationToken = default)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdAsync(articleToUpdate.Id);

        if (article is null)
        {
            throw new Exception("Entity not found.");
        }

        article = _mapper.Map<Article>(articleToUpdate);
        _repositoryManager.ArticleRepository.Update(article);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await _repositoryManager.ArticleRepository.GetByIdAsync(id, cancellationToken);

        if (article is null)
        {
            throw new Exception("Entity not found..");
        }

        _repositoryManager.ArticleRepository.Delete(article);
        await _repositoryManager.SaveAsync(cancellationToken);
    }
}
