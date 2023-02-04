using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

#nullable disable
public class AuthorizationRequest
{
    [Required(ErrorMessage = "Login is required"), MinLength(1)]
    public string Login { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
