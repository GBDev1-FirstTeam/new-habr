#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class RegistrationRequest
{
    [Required(ErrorMessage = "UserName is required"), MinLength(3), MaxLength(20)]
    [RegularExpression("[a-zA-Z0-9]+", ErrorMessage = "Allowed characters are: a-z and numbers")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Secure question is required")]
    [Range(1, 1000)]
    public int SecurityQuestionId { get; set; }

    [MinLength(1)]
    [Required(ErrorMessage = "Secure answer is required")]
    public string SecurityQuestionAnswer { get; set; }
}
