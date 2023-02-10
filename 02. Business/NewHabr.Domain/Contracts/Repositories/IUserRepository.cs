﻿using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User?> GetUserByLoginAsync(string login, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<User>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<User>> GetDeletedUsersAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<User>> GetBannedUsersAsync(CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
