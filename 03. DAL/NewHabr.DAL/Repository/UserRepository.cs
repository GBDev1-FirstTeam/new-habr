using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository.Impl;
public class UserRepository : ReporitoryBase<User, Guid>, IUserRepository
{
    public UserRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<User>> GetBannedUsersAsync(CancellationToken cancellationToken = default)
    {
        return await FindByCondition(u => u.Banned && !u.Deleted).ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetById(id, trackChanges).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<User>> GetByRoleIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<User>> GetDeletedUsersAsync(CancellationToken cancellationToken = default)
    {
        return await FindByCondition(u => u.Deleted).ToListAsync(cancellationToken);
    }

    public async Task<User?> GetUserByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(u => u.UserName == login && !u.Deleted).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<int> GetUsersCountWithSecureQuestionId(int id, CancellationToken cancellationToken)
    {
        return await FindByCondition(u => u.SecureQuestionId == id)
            .CountAsync(cancellationToken);
    }
}
