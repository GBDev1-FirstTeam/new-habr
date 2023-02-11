using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

namespace NewHabr.Domain.Contracts;

public interface ITagService
{
    Task<IReadOnlyCollection<TagDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <exception cref="TagAlreadyExistsException"></exception>
    Task CreateAsync(CreateTagRequest request, CancellationToken cancellationToken = default);

    /// <exception cref="TagNotFoundException"></exception>
    /// <exception cref="TagAlreadyExistsException"></exception>
    Task UpdateAsync(int id, UpdateTagRequest tagToUpdate, CancellationToken cancellationToken = default);

    /// <exception cref="TagNotFoundException"></exception>
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
}
