using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ITagRepository : IRepository<Tag>
{
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    new Task<IReadOnlyCollection<Tag>> GetAllAsync(CancellationToken cancellationToken = default);
}
