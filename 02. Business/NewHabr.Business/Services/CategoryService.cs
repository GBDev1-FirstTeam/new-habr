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

        var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

        return categoriesDto;
    }

    public async Task UpdateAsync(CategoryDto categoryToUpdate, CancellationToken cancellationToken = default)
    {
        var category = _repositoryManager.CategoryRepository.FindByCondition(c => c.Id == categoryToUpdate.Id && !c.Deleted, true)
            .SingleOrDefault();

        if (category is null)
        {
            throw new Exception();
        }

        _mapper.Map(categoryToUpdate, category);

        _repositoryManager.CategoryRepository.Update(category);
        await _repositoryManager.SaveAsync(cancellationToken);
    }
}
