namespace NewHabr.Domain.Models;

public class User // при внедрении аутентификации этот класс будет наследоваться от Identity<User>
{
    public string Login { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    public int? Age { get; set; }
    public string? Description { get; set; }

    public List<Comment> Comments { get; set; } = new();
    public List<UserNotification> Notifications { get; set; } = new();
    public List<LikedArticle> LikedArticles { get; set; } = new();
    public List<LikedComment> LikedComments { get; set; } = new();
    public List<LikedUser> LikedUsers { get; set; } = new();

    public bool Banned { get; set; }
    public bool Deleted { get; set; }
    public DateTimeOffset? BanExpiratonDate { get; set; }
    public DateTimeOffset? BannedAt { get; set; }
    public SecureQuestion SecureQuestion { get; set; } = null!;
    public string SecureAnswer { get; set; } = string.Empty;
}
