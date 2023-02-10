using System;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ISecureQuestionsService
{
    Task<ICollection<SecureQuestionDto>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(SecureQuestionCreateRequest request, CancellationToken cancellationToken);
    Task UpdateAsync(int id, SecureQuestionUpdateRequest request, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}

