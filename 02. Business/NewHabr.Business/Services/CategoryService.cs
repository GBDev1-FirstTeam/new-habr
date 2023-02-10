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

    public async Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _repositoryManager.CategoryRepository.GetAvaliableAsync(cancellationToken: cancellationToken);
        return _mapper.Map<List<CategoryDto>>(categories);
    }
    public async Task<int> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = _repositoryManager.CategoryRepository.FindByCondition(c => c.Name == request.Name && !c.Deleted).FirstOrDefault();

        if (category is not null)
        {
            throw new CategoryAlreadyExistsException();
        }

        var newCategory = _mapper.Map<Category>(request);
        _repositoryManager.CategoryRepository.Create(newCategory);
        await _repositoryManager.SaveAsync(cancellationToken);
        return newCategory.Id;
    }
    public async Task UpdateAsync(int id, UpdateCategoryRequest categoryToUpdate, CancellationToken cancellationToken = default)
    {
        var targetCategory = await _repositoryManager.CategoryRepository.GetByIdAsync(id, trackChanges: true, cancellationToken);

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
        targetCategory.Id = id;

        await _repositoryManager.SaveAsync(cancellationToken);
    }
    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _repositoryManager.CategoryRepository.GetByIdIncludeAsync(id, trackChanges: true, cancellationToken);

        if (category is null)
        {
            throw new CategoryNotFoundException();
        }

        category.Articles = null;
        category.Deleted = true;

        await _repositoryManager.SaveAsync(cancellationToken);
    }
}
