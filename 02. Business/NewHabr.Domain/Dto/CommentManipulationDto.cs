#nullable disable

using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public abstract class CommentManipulationDto
{
    [Required]
    public string Text { get; set; }
}

