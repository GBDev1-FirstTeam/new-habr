#nullable disable

using System;
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public abstract class SecureQuestionForManipulationDto
{
    [Required, MinLength(5)]
    public string Question { get; set; }
}

