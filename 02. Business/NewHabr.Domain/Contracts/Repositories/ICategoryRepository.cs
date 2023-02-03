using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ICategoryRepository : IRepository<Category>
{
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    new Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default);
}
