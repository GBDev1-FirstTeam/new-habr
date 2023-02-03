using AutoMapper;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

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

    public async Task CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        _repositoryManager.TagRepository.Create(new()
        {
            Name = name,
        });

        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await _repositoryManager.TagRepository.DeleteByIdAsync(id, cancellationToken);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TagDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var tags = await _repositoryManager.TagRepository.GetAllAsync(cancellationToken);

        if (tags is null)
        {
            return new List<TagDto>();
        }

        var tagsDto = _mapper.Map<List<TagDto>>(tags);

        return tagsDto ?? new List<TagDto>();
    }

    public async Task UpdateAsync(TagDto tagToUpdate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tagToUpdate, nameof(tagToUpdate));

        var tag = _mapper.Map<Tag>(tagToUpdate);

        if (tag == null)
        {
            throw new AutoMapperMappingException();
        }

        _repositoryManager.TagRepository.Update(tag);
        await _repositoryManager.SaveAsync(cancellationToken);
    }
}
