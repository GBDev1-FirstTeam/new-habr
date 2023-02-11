#nullable disable

using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public abstract class CategoryManipulationDto
{
    [Required, MinLength(3), MaxLength(100)]
    public string Name { get; set; }
}

