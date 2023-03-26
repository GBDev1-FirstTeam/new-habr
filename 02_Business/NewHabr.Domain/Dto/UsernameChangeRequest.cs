#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class UsernameChangeRequest
{
    [Required(ErrorMessage = "UserName is required"), MinLength(3), MaxLength(20)]
    [RegularExpression("[a-zA-Z0-9]+", ErrorMessage = "Allowed characters are: a-z and numbers")]
    public string Username { get; set; }
}
