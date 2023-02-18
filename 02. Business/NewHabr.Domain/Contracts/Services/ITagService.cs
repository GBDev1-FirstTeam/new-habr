using NewHabr.Domain.Dto;

namespace NewHabr.Domain.Contracts;

public interface ITagService
{
    Task<IReadOnlyCollection<TagDto>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(TagCreateRequest request, CancellationToken cancellationToken);
    Task UpdateAsync(int id, TagUpdateRequest tagToUpdate, CancellationToken cancellationToken);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
}
