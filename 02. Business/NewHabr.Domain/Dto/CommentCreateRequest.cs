#nullable disable

using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class CommentCreateRequest : CommentManipulationDto
{
    [Required]
    public Guid ArticleId { get; set; }
}
