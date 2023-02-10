#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class CreateTagRequest
{
    [Required, MinLength(3), MaxLength(50)]
    public string Name { get; set; }
}
