using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

#nullable disable
public class RegistrationRequest
{
    [Required(ErrorMessage = "Login is required")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Secure question is required")]
    public int SecurityQuestionId { get; set; }

    [MinLength(1)]
    [Required(ErrorMessage = "Secure answer is required")]
    public string SecurityQuestionAnswer { get; set; }
}
