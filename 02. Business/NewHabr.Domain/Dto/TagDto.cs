#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewHabr.Domain.Dto;

public class TagDto
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [Required, NotNull, MinLength(3), MaxLength(50)]
    public string Name { get; set; }
}
