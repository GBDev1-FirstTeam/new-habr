using NewHabr.Domain.Dto;

namespace NewHabr.Domain.Contracts;

public interface ITagService
{
    Task<IReadOnlyCollection<TagDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task CreateAsync(string name, CancellationToken cancellationToken = default);
    Task UpdateAsync(TagDto tagToUpdate, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default);
}
