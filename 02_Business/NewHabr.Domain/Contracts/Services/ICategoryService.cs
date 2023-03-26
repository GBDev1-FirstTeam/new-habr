using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

namespace NewHabr.Domain.Contracts;

public interface ICategoryService
{
    Task<CategoryDto> GetByIdAsync(int categoryId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<CategoryDto> CreateAsync(CategoryCreateRequest request, CancellationToken cancellationToken);

    Task UpdateAsync(int id, CategoryUpdateRequest categoryToUpdate, CancellationToken cancellationToken);

    Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
}
