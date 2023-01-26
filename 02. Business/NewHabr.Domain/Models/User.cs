using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class User // при внедрении аутентификации этот класс будет наследоваться от Identity<User>
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(30), MinLength(2)]
    public string Login { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? FirstName { get; set; }

    [MaxLength(30)]
    public string? LastName { get; set; }

    [MaxLength(30)]
    public string? Patronymic { get; set; }

    [Range(0, 120)]
    public int? Age { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }


    public ICollection<Comment> Comments { get; set; } = null!;

    public ICollection<UserNotification> Notifications { get; set; } = null!;

    public ICollection<LikedArticle> LikedArticles { get; set; } = null!;

    public ICollection<LikedComment> LikedComments { get; set; } = null!;

    public ICollection<LikedUser> LikedUsers { get; set; } = null!;

    public ICollection<LikedUser> ReceivedLikes { get; set; } = null!;


    public bool Banned { get; set; }

    public bool Deleted { get; set; }

    public DateTimeOffset? BanExpiratonDate { get; set; }

    public DateTimeOffset? BannedAt { get; set; }

    [Required]
    public SecureQuestion SecureQuestion { get; set; } = null!;

    [Required]
    public string SecureAnswer { get; set; } = string.Empty;
}
