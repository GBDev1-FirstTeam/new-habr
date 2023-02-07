using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;

namespace NewHabr.Domain.Contracts;

public interface ICategoryService
{
    Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <exception cref="CategoryAlreadyExistsException"></exception>
    Task CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default);

    /// <exception cref="CategoryNotFoundException"></exception>
    /// <exception cref="CategoryAlreadyExistsException"></exception>
    Task UpdateAsync(CategoryDto categoryToUpdate, CancellationToken cancellationToken = default);

    /// <exception cref="CategoryNotFoundException"></exception>
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
}
