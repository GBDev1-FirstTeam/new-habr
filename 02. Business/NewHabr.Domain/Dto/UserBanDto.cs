using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class UserBanDto
{
    [Required, StringLength(200, MinimumLength = 10)]
    public string BanReason { get; set; }
}
