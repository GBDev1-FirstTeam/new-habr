﻿using AutoMapper;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
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

    public async Task<IReadOnlyCollection<TagDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var tags = await _repositoryManager.TagRepository.GetAvaliableAsync(cancellationToken: cancellationToken);
        return _mapper.Map<List<TagDto>>(tags);
    }
    public async Task CreateAsync(TagCreateRequest request, CancellationToken cancellationToken = default)
    {
        var tag = _repositoryManager
            .TagRepository
            .GetByNameAsync(request.Name, false, cancellationToken);

        if (tag is not null)
        {
            throw new TagAlreadyExistsException();
        }

        var newTag = _mapper.Map<Tag>(request);
        _repositoryManager.TagRepository.Create(newTag);
        await _repositoryManager.SaveAsync(cancellationToken);
    }
    public async Task UpdateAsync(int id, TagUpdateRequest tagToUpdate, CancellationToken cancellationToken = default)
    {
        var targetTag = await _repositoryManager.TagRepository.GetByIdAsync(id, trackChanges: true, cancellationToken);

        if (targetTag is null)
        {
            throw new TagNotFoundException();
        }

        var tagWithSameName = await _repositoryManager
            .TagRepository
            .GetByNameAsync(tagToUpdate.Name, false, cancellationToken);

        if (tagWithSameName is not null)
        {
            throw new TagAlreadyExistsException();
        }

        _mapper.Map(tagToUpdate, targetTag);
        targetTag.Id = id;

        await _repositoryManager.SaveAsync(cancellationToken);
    }
    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var tag = await _repositoryManager.TagRepository.GetByIdIncludeAsync(id, trackChanges: true, cancellationToken);

        if (tag is null)
        {
            throw new TagNotFoundException();
        }

        _repositoryManager.TagRepository.Delete(tag);
        await _repositoryManager.SaveAsync(cancellationToken);
    }
}
