using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ITagRepository : IRepository<Tag, int>
{
    Task<Tag?> GetByIdAsync(int id, bool trackChanges, CancellationToken cancellationToken);
    Task<Tag?> GetByIdIncludeAsync(int id, bool trackChanges, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Tag>> GetAvaliableAsync(bool trackChanges, CancellationToken cancellationToken);
    Task<Tag?> GetByNameAsync(string name, bool trackChanges, CancellationToken cancellationToken);
}
