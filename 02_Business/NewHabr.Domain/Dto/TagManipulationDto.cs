#nullable disable

using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public abstract class TagManipulationDto
{
    [Required, MinLength(3), MaxLength(50)]
    public string Name { get; set; }

}

