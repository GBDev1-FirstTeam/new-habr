using NewHabr.Domain.Dto;

namespace NewHabr.Domain.Contracts;

public interface ICategoryService
{
    Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(CategoryCreateRequest request, CancellationToken cancellationToken);
    Task UpdateAsync(int id, CategoryUpdateRequest categoryToUpdate, CancellationToken cancellationToken);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
}
