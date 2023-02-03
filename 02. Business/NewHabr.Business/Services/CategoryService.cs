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

    public async Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _repositoryManager.CategoryRepository.GetAllAsync(cancellationToken);

        if (categories is null)
        {
            return new List<CategoryDto>();
        }

        var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

        return categoriesDto ?? new List<CategoryDto>();
    }

    public async Task UpdateAsync(CategoryDto categoryToUpdate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(categoryToUpdate, nameof(categoryToUpdate));

        var category = _mapper.Map<Category>(categoryToUpdate);

        if (category == null)
        {
            throw new AutoMapperMappingException();
        }

        _repositoryManager.CategoryRepository.Update(category);
        await _repositoryManager.SaveAsync(cancellationToken);
    }
}
