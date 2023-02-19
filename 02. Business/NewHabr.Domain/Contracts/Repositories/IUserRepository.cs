using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User?> GetUserByLoginAsync(string login, bool trackChanges, CancellationToken cancellationToken);
    Task<ICollection<User>> GetUsersByLoginAsync(ICollection<string> usernames, bool trackChanges, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<User>> GetByRoleIdAsync(int roleId, bool trackChanges, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<User>> GetDeletedUsersAsync(bool trackChanges, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<User>> GetBannedUsersAsync(bool trackChanges, CancellationToken cancellationToken);
    Task<int> GetUsersCountWithSecureQuestionIdAsync(int id, bool trackChanges, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);
    Task<ICollection<UserLikedUser>> GetUserLikedUsersAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);
    Task<int> GetReceivedLikesCountAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);
    Task<User?> GetByIdWithLikesAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);
}
