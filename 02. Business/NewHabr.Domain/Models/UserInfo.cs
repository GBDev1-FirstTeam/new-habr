using System;

namespace NewHabr.Domain.Models;

public class UserInfo
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Patronymic { get; set; }

    public DateTimeOffset? BirthDay { get; set; }

    public string? Description { get; set; }

    public bool Banned { get; set; }

    public string? BanReason { get; set; }

    public DateTimeOffset? BanExpiratonDate { get; set; }

    public DateTimeOffset? BannedAt { get; set; }

    public int ReceivedLikes { get; set; }

    public bool ILiked { get; set; }

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

