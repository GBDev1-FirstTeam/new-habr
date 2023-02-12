using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace NewHabr.Business.Services;

public class SecureQuestionsService : ISecureQuestionsService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger<SecureQuestionsService> _logger;
    private readonly IMapper _mapper;


    public SecureQuestionsService(IRepositoryManager repositoryManager, ILogger<SecureQuestionsService> logger, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
        _mapper = mapper;
    }


    public async Task CreateAsync(SecureQuestionCreateRequest request, CancellationToken cancellationToken)
    {
        await EnsureQuestionNotExists(request, cancellationToken);

        var newQuestion = _mapper.Map<SecureQuestion>(request);
        _repositoryManager.SecureQuestionsRepository.Create(newQuestion);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task<ICollection<SecureQuestionDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var questions = await _repositoryManager
            .SecureQuestionsRepository
            .GetAllAsync(cancellationToken);
        var questionsDto = _mapper.Map<List<SecureQuestionDto>>(questions);
        return questionsDto;
    }

    public async Task UpdateAsync(int id, SecureQuestionUpdateRequest request, CancellationToken cancellationToken)
    {
        await EnsureQuestionNotExists(request, cancellationToken);

        var sq = await GetIfExistsAndNotInUse(id, cancellationToken);

        _mapper.Map(request, sq);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var sq = await GetIfExistsAndNotInUse(id, cancellationToken);

        _repositoryManager.SecureQuestionsRepository.Delete(sq);
        await _repositoryManager.SaveAsync(cancellationToken);
    }


    private async Task EnsureQuestionNotExists(SecureQuestionForManipulationDto request, CancellationToken cancellationToken)
    {
        var allQuestions = await GetAllAsync(cancellationToken);
        if (allQuestions.Any(q => q.Question.Equals(request.Question)))
            throw new InvalidOperationException("Question already exists");
    }

    private async Task<SecureQuestion> GetIfExistsAndNotInUse(int id, CancellationToken cancellationToken)
    {
        var sq = await _repositoryManager
            .SecureQuestionsRepository
            .GetByIdAsync(id, true, cancellationToken);
        if (sq is null)
            throw new SecureQuestionNotFoundException();

        // users count using secure question with id
        var usersCount = await _repositoryManager
            .UserRepository
            .GetUsersCountWithSecureQuestionIdAsync(id, cancellationToken);

        if (usersCount > 0)
            throw new InvalidOperationException($"SecureQuestion({id}) is in use.");
        return sq;
    }
}

