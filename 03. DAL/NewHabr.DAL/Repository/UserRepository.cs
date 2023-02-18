using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository.Impl;
public class UserRepository : RepositoryBase<User, Guid>, IUserRepository
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

    public async Task<User?> GetByIdWithLikesAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await GetById(id, trackChanges)
            .Include(u => u.ReceivedLikes)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<User>> GetByRoleIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<User>> GetDeletedUsersAsync(CancellationToken cancellationToken = default)
    {
        return await FindByCondition(u => u.Deleted).ToListAsync(cancellationToken);
    }

    public async Task<int> GetReceivedLikesCountAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await GetById(userId, false)
            .Include(e => e.ReceivedLikes)
            .Select(user => user.ReceivedLikes.Count)
            .SingleAsync(cancellationToken);
    }

    public async Task<User?> GetUserByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(u => u.UserName == login && !u.Deleted).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<ICollection<User>> GetUsersByLoginAsync(ICollection<string> usernames, bool trackChanges, CancellationToken cancellationToken)
    {
        return await FindByCondition(user => !user.Deleted && usernames.Contains(user.UserName), trackChanges)
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<UserLikedUser>> GetUserLikedUsersAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await FindByCondition(user => !user.Deleted)
            .Include(user => user.ReceivedLikes)
            .Where(author => author.ReceivedLikes.Any(u => u.Id == userId))
            .Select(row => new UserLikedUser
            {
                UserId = userId, //кто поставил лайк
                Id = row.Id,     //кому поставили лайк
                UserName = row.UserName
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUsersCountWithSecureQuestionIdAsync(int id, CancellationToken cancellationToken)
    {
        return await FindByCondition(u => u.SecureQuestionId == id)
            .CountAsync(cancellationToken);
    }
}
