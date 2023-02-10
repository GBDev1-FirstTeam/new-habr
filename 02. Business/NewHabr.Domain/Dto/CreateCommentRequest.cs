#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class CreateCommentRequest
{
    [Required]
    public Guid ArticleId { get; set; }

    [Required]
    public string Text { get; set; }
}
