using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ICategoryRepository : IRepository<Category, int>
{
    Task<Category?> GetByIdAsync(int id, bool trackChanges, CancellationToken cancellationToken);
    Task<Category?> GetByIdIncludeAsync(int id, bool trackChanges, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Category>> GetAvaliableAsync(bool trackChanges, CancellationToken cancellationToken);
    Task<Category?> GetByNameAsync(string name, bool trackChanges, CancellationToken cancellationToken);
}
