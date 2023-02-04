#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewHabr.Domain.Dto;

public class CreateTagRequest
{
    [Required, NotNull, MinLength(3), MaxLength(50)]
    public string Name { get; set; }
}
