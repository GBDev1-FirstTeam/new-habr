using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default);
}
