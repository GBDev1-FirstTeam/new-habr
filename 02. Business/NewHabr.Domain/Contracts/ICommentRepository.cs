﻿using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IReadOnlyCollection<Comment>> GetByUserIdAsync(
        Guid userId,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Comment>> GetByArticleIdAsync(
        Guid articleId,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);
}
