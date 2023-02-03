using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ITagRepository : IRepository<Tag>
{
    Task<IReadOnlyCollection<Tag>> GetAllAsync(CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default);
}
