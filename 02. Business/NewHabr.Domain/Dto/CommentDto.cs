#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class CommentDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid ArticleId { get; set; }

    [Required]
    public string Text { get; set; }

    public long CreatedAt { get; set; }

    public long ModifiedAt { get; set; }
}
