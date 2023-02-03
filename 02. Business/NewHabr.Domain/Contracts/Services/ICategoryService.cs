using NewHabr.Domain.Dto;
using AutoMapper;

namespace NewHabr.Domain.Contracts;

public interface ICategoryService
{
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AutoMapperMappingException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task UpdateAsync(CategoryDto categoryToUpdate, CancellationToken cancellationToken = default);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
}
