using AutoMapper;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
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

    public async Task CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = _repositoryManager.CategoryRepository.FindByCondition(c => c.Name == request.Name && !c.Deleted).FirstOrDefault();

        if (category is not null)
        {
            throw new CategoryAlreadyExistsException();
        }

        var newCategory = _mapper.Map<Category>(request);
        _repositoryManager.CategoryRepository.Create(newCategory);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _repositoryManager.CategoryRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

        if (category is null)
        {
            throw new CategoryNotFoundException();
        }

        _repositoryManager.CategoryRepository.Delete(category);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _repositoryManager.CategoryRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task UpdateAsync(CategoryDto categoryToUpdate, CancellationToken cancellationToken = default)
    {
        var targetCategory = await _repositoryManager.CategoryRepository.GetByIdAsync(categoryToUpdate.Id, cancellationToken: cancellationToken);

        if (targetCategory is null)
        {
            throw new CategoryNotFoundException();
        }

        var categoryWithSameName = _repositoryManager.CategoryRepository
            .FindByCondition(c => c.Name == categoryToUpdate.Name && !c.Deleted).FirstOrDefault();

        if (categoryWithSameName is not null)
        {
            throw new CategoryAlreadyExistsException();
        }

        _mapper.Map(categoryToUpdate, targetCategory);

        _repositoryManager.CategoryRepository.Update(targetCategory);
        await _repositoryManager.SaveAsync(cancellationToken);
    }
}
