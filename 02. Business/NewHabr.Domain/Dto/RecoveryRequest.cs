using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

#nullable disable
public class RecoveryRequest
{
    [Required(ErrorMessage = "UserName is required"), MinLength(3)]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Secure question is required")]
    [Range(1, 1000)]
    public int SecureQuestionId { get; set; }    
    [Required(ErrorMessage = "Secure answer is required"), MinLength(1)]
    public string Answer { get; set; }
}
