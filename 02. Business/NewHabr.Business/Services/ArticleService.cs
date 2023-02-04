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
        var article = await _repositoryManager.ArticleRepository.GetByIdAsync(id, cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException();
        }

        return _mapper.Map<ArticleDto>(article);
    }

    public async Task<IReadOnlyCollection<ArticleDto>> GetUnpublishedAsync(CancellationToken cancellationToken = default)
    {
        var articles = await _repositoryManager.ArticleRepository.GetUnpublishedAsync(cancellationToken);
        return _mapper.Map<List<ArticleDto>>(articles);
    }

    public async Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        var articles = await _repositoryManager.ArticleRepository.GetDeletedAsync(cancellationToken);
        return _mapper.Map<List<ArticleDto>>(articles);
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
            throw new ArticleNotFoundException();
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
            throw new ArticleNotFoundException();
        }

        _repositoryManager.ArticleRepository.Delete(article);
        await _repositoryManager.SaveAsync(cancellationToken);
    }
}
