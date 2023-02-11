using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ICategoryRepository : IRepository<Category, int>
{
    Task<Category?> GetByIdAsync(int id, bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<Category?> GetByIdIncludeAsync(int id, bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Category>> GetAvaliableAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<Category?> GetByNameAsync(string name, bool trackChanges, CancellationToken cancellationToken = default);
}
