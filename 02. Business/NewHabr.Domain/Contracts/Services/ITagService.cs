using NewHabr.Domain.Dto;
using AutoMapper;

namespace NewHabr.Domain.Contracts;

public interface ITagService
{
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<IReadOnlyCollection<TagDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task CreateAsync(CreateTagRequest request, CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task UpdateAsync(TagDto tagToUpdate, CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
}
