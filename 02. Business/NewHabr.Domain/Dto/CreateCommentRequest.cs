using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewHabr.Domain.Dto;
public class CreateCommentRequest
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid ArticleId { get; set; }
    [NotNull]
    public string Text { get; set; }
    public long CreatedAt { get; set; }
}
