#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class UpdateCommentRequest
{
    [Required]
    public string Text { get; set; }
}
