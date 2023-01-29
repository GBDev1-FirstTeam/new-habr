using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewHabr.DAL.EF;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository.Impl;
public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly ApplicationContext _context;

    public UserRepository(ILogger<UserRepository> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Guid> Create(User data)
    {
        await _context.Users.AddAsync(data);
        await _context.SaveChangesAsync();
        return data.Id;
    }

    public async Task<int> Delete(Guid id)
    {
        User user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        if (user == null)
        {
            _logger.LogInformation($"User {id} not found");
            return 0;
        }
        user.Deleted = true;
        await _context.SaveChangesAsync();
        
        return 1;
    }

    public async Task<IReadOnlyCollection<User>> GetAll()
    {        
        return await _context.Users.ToListAsync();
    }

    public async Task<IReadOnlyCollection<User>> GetBannedUsers()
    {        
        return await _context.Users.Where(user => user.Banned == true).ToListAsync();
    }

    public async Task<IReadOnlyCollection<User>> GetByBirthday(DateTime birthDate)
    {        
        return await _context.Users.Where(user => user.BirthDay == birthDate).ToListAsync(); 
    }

    public async Task<IReadOnlyCollection<User>> GetByFirstName(string firstName)
    {
        return await _context.Users.Where(user => user.FirstName.ToLower() == firstName.ToLower()).ToListAsync();
    }

    public async Task<User> GetById(Guid id)
    {
        User user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        if (user is null)
        {
            _logger.LogInformation($"User {id} not found");
            throw new ArgumentNullException(nameof(user));
        }
        return user;
    }

    public async Task<IReadOnlyCollection<User>> GetByLastName(string lastName)
    {
        return await _context.Users.Where(user => user.LastName.ToLower() == lastName.ToLower()).ToListAsync();
    }

    public async Task<IReadOnlyCollection<User>> GetByPatronimyc(string patronimyc)
    {
        return await _context.Users.Where(user => user.Patronymic == patronimyc).ToListAsync();
    }

    public async Task<IReadOnlyCollection<User>> GetByRoleId(int roleId)
    {       
        return await _context.Users.Where(user => user.Role == roleId).ToListAsync();
    }

    public async Task<IReadOnlyCollection<User>> GetDeletedUsers()
    {        
        return await _context.Users.Where(user => user.Deleted == true).ToListAsync();
    }

    public async Task<User> GetUserByLogin(string login)
    {  
        User user = await _context.Users.FirstOrDefaultAsync(user => user.Login == login);
        if (user is null)
        {
            _logger.LogInformation($"User with login \"{login}\" not found");
            throw new ArgumentNullException(nameof(user));
        }
        return user;
    }

    public async Task<int> Update(User data)
    {
        User user = await _context.Users.FirstOrDefaultAsync(user => user.Id == data.Id);
        if (user is null)
        {
            _logger.LogInformation($"User {data.Id} not found");
            return 0;
        }

        user.Login = data.Login;
        user.FirstName = data.FirstName;
        user.LastName = data.LastName;
        user.Patronymic = data.Patronymic;
        user.BirthDay = data.BirthDay;
        user.SecureQuestion = data.SecureQuestion;
        user.SecureQuestionId = data.SecureQuestionId;
        user.SecureAnswer = data.SecureAnswer;
        user.ReceivedLikes = data.ReceivedLikes;
        user.Role = data.Role;
        user.Banned = data.Banned;
        user.BannedAt = data.BannedAt;
        user.BanReason = data.BanReason;
        user.BanExpiratonDate = data.BanExpiratonDate;
        user.Comments = data.Comments;
        user.Description = data.Description;
        user.LikedComments = data.LikedComments;
        user.LikedArticles = data.LikedArticles;
        user.LikedUsers = data.LikedUsers;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return 1;
    }
}
