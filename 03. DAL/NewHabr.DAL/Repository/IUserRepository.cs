﻿using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByLoginAsync(string login, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<User>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<User>> GetDeletedUsersAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<User>> GetBannedUsersAsync(CancellationToken cancellationToken = default);
}
