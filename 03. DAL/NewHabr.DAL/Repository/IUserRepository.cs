using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;
public interface IUserRepository : IRepository<User, Guid>
{
    Task<User> GetUserByLogin(string login);
    Task<IReadOnlyCollection<User>> GetByFirstName(string firstName);
    Task<IReadOnlyCollection<User>> GetByLastName(string lastName);
    Task<IReadOnlyCollection<User>> GetByPatronimyc(string patronimyc);
    Task<IReadOnlyCollection<User>> GetByBirthday(DateTime birthDate);
    Task<IReadOnlyCollection<User>> GetByRoleId(int roleId);
    Task<IReadOnlyCollection<User>> GetDeletedUsers();
    Task<IReadOnlyCollection<User>> GetBannedUsers();
}
