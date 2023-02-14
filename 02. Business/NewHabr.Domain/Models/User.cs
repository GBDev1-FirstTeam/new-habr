#nullable disable
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NewHabr.Domain.Models;

public class User : IdentityUser<Guid>, IEntity<Guid>
{
    [MaxLength(30)]
    public string FirstName { get; set; }

    [MaxLength(30)]
    public string LastName { get; set; }

    [MaxLength(30)]
    public string Patronymic { get; set; }

    public DateTime? BirthDay { get; set; }

    [MaxLength(200)]
    public string Description { get; set; }

    public ICollection<Comment> Comments { get; set; }

    public ICollection<Notification> Notifications { get; set; }

    public ICollection<LikedArticle> LikedArticles { get; set; }

    public ICollection<LikedComment> LikedComments { get; set; }

    public ICollection<LikedUser> LikedUsers { get; set; }

    public ICollection<LikedUser> ReceivedLikes { get; set; }

    public bool Banned { get; set; }

    [MaxLength(200)]
    public string BanReason { get; set; }

    public bool Deleted { get; set; }

    public DateTimeOffset? BanExpiratonDate { get; set; }

    public DateTimeOffset? BannedAt { get; set; }

    [Required]
    public int SecureQuestionId { get; set; }

    public SecureQuestion SecureQuestion { get; set; }

    [Required]
    public string SecureAnswer { get; set; }

    public int? Age
    {
        get
        {
            if (BirthDay is null || BirthDay > DateTime.Now)
                return null;

            return (DateTime.Now - BirthDay).Value.Days / 365;
        }
    }

}
