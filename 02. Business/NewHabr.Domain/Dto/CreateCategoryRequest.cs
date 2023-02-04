#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewHabr.Domain.Dto;

public class CreateCategoryRequest
{
    [Required, NotNull, MinLength(3), MaxLength(100)]
    public string Name { get; set; }
}
