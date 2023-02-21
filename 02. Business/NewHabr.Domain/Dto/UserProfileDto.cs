#nullable disable

namespace NewHabr.Domain.Dto;

public class UserProfileDto : UserForManipulationDto
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public int? Age { get; set; }

    public bool Banned { get; set; }

    public string BanReason { get; set; }

    public long? BanExpiratonDate { get; set; }

    public long? BannedAt { get; set; }

    public int ReceivedLikes { get; set; }

    public bool ILiked { get; set; }
}
