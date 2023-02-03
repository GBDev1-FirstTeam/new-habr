using NewHabr.Domain.Dto;

namespace NewHabr.Domain.Contracts;

public interface ICategoryService
{
    Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task CreateAsync(string name, CancellationToken cancellationToken = default);
    Task UpdateAsync(CategoryDto categoryToUpdate, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default);
}
