using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

#nullable disable
public class AuthorizationRequest
{
    [Required(ErrorMessage = "UserName is required"), MinLength(1)]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
