using NewHabr.Domain.Dto;
using AutoMapper;

namespace NewHabr.Domain.Contracts;

public interface IArticleService
{
    /// <summary></summary>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    /// <returns></returns>
    Task<IReadOnlyCollection<ArticleDto>> GetByTitleAsync(string title, CancellationToken cancellationToken = default);

    /// <summary></summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    /// <returns></returns>
    Task<IReadOnlyCollection<ArticleDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary></summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    /// <returns></returns>
    Task<IReadOnlyCollection<ArticleDto>> GetPublishedAsync(CancellationToken cancellationToken = default);

    /// <summary></summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    /// <returns></returns>
    Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default);

    /// <summary></summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    /// <returns></returns>
    Task CreateAsync(CreateArticleRequest request, CancellationToken cancellationToken = default);

    /// <summary></summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    /// <returns></returns>
    Task UpdateAsync(ArticleDto updatedArticle, CancellationToken cancellationToken = default);

    /// <summary></summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    /// <returns></returns>
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
