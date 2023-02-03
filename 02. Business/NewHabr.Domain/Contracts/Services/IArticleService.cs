using NewHabr.Domain.Dto;
using AutoMapper;

namespace NewHabr.Domain.Contracts;

public interface IArticleService
{
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<IReadOnlyCollection<ArticleDto>> GetByTitleAsync(string title, CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<IReadOnlyCollection<ArticleDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<IReadOnlyCollection<ArticleDto>> GetPublishedAsync(CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<IReadOnlyCollection<ArticleDto>> GetDeletedAsync(CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task CreateAsync(CreateArticleRequest request, CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task UpdateAsync(ArticleDto articleToUpdate, CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
