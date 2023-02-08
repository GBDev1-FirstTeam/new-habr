﻿using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ICommentRepository : IRepository<Comment, Guid>
{
    Task<IReadOnlyCollection<Comment>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Comment>> GetByArticleIdAsync(Guid articleId, CancellationToken cancellationToken = default);
    Task<ICollection<UserComment>> GetUserCommentAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ICollection<UserLikedComment>> GetUserLikedCommentsAsync(Guid userId, CancellationToken cancellationToken);
}