using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ITagRepository : IRepository<Tag, int>
{
    Task<Tag?> GetByIdAsync(int id, bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<Tag?> GetByIdIncludeAsync(int id, bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Tag>> GetAvaliableAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
}
