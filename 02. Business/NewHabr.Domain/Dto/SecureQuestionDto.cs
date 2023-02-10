#nullable disable

using System;
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class SecureQuestionDto
{
    public int Id { get; set; }
    public string Question { get; set; }
}

