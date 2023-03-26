#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class UserPasswordChangeRequest
{
    [Required(ErrorMessage = "New password is required")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Current password is required")]
    public string CurrentPassword { get; set; }
}
