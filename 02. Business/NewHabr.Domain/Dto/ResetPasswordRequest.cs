using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;
public class ResetPasswordRequest
{
    [Required]
    public string Token { get; set; }

    [Required(ErrorMessage = "UserName is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string NewPassword { get; set; }
}
