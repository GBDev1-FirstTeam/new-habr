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

    public async Task CreateAsync(CreateTagRequest request, CancellationToken cancellationToken = default)
    {
        var tag = _repositoryManager.TagRepository.FindByCondition(c => c.Name == request.Name && !c.Deleted).FirstOrDefault();

        if (tag is not null)
        {
            throw new Exception("Entity already exists.");
        }

        var newTag = _mapper.Map<Tag>(request);
        _repositoryManager.TagRepository.Create(newTag);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var tag = await _repositoryManager.TagRepository.GetByIdAsync(id, cancellationToken);

        if (tag is null)
        {
            throw new Exception("Entity not found.");
        }

        _repositoryManager.TagRepository.Delete(tag);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TagDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var tags = await _repositoryManager.TagRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<TagDto>>(tags);
    }

    public async Task UpdateAsync(TagDto tagToUpdate, CancellationToken cancellationToken = default)
    {
        var tag = await _repositoryManager.TagRepository.GetByIdAsync(tagToUpdate.Id, cancellationToken);

        if (tag is null)
        {
            throw new Exception("Entity not found.");
        }

        _mapper.Map(tagToUpdate, tag);

        _repositoryManager.TagRepository.Update(tag);
        await _repositoryManager.SaveAsync(cancellationToken);
    }
}
