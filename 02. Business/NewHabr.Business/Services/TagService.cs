using AutoMapper;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;

namespace NewHabr.Business.Services;

public class TagService : ITagService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public TagService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<TagDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var tags = await _repositoryManager.TagRepository.GetAllAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(tags, nameof(tags));

        var tagsDto = _mapper.Map<List<TagDto>>(tags);

        ArgumentNullException.ThrowIfNull(tagsDto, nameof(tagsDto));

        return tagsDto;
    }
}
