using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User?> GetUserByLoginAsync(string login, CancellationToken cancellationToken = default);
    Task<ICollection<User>> GetUsersByLoginAsync(ICollection<string> usernames, bool trackChanges, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<User>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<User>> GetDeletedUsersAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<User>> GetBannedUsersAsync(CancellationToken cancellationToken = default);
    Task<int> GetUsersCountWithSecureQuestionIdAsync(int id, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);
    Task<ICollection<UserLikedUser>> GetUserLikedUsersAsync(Guid userId, CancellationToken cancellationToken);
    Task<int> GetReceivedLikesCountAsync(Guid userId, CancellationToken cancellationToken);
    Task<User?> GetByIdWithLikesAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default);
}
