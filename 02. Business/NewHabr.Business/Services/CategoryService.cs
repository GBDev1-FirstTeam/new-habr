using AutoMapper;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Business.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public CategoryService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        _repositoryManager.CategoryRepository.Create(new()
        {
            Name = name,
        });
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await _repositoryManager.CategoryRepository.DeleteByIdAsync(id, cancellationToken);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        await _repositoryManager.CategoryRepository.DeleteByNameAsync(name, cancellationToken);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _repositoryManager.CategoryRepository.GetAllAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(categories, nameof(categories));

        var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

        ArgumentNullException.ThrowIfNull(categoriesDto, nameof(categoriesDto));

        return categoriesDto;
    }

    public async Task UpdateAsync(CategoryDto categoryToUpdate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(categoryToUpdate, nameof(categoryToUpdate));

        var category = _mapper.Map<Category>(categoryToUpdate);

        ArgumentNullException.ThrowIfNull(category, nameof(category));

        _repositoryManager.CategoryRepository.Update(category);
        await _repositoryManager.SaveAsync(cancellationToken);
    }
}
