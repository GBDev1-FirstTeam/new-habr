using AutoMapper;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;

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
        var categories = await _repositoryManager.CategoryRepository.GetAllAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(categories, nameof(categories));

        var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

        ArgumentNullException.ThrowIfNull(categoriesDto, nameof(categoriesDto));

        return categoriesDto;
    }
}
