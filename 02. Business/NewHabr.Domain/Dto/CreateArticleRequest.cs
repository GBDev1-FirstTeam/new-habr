#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewHabr.Domain.Dto;

public class CreateArticleRequest
{
    [Required]
    public Guid UserId { get; set; }

    [NotNull, MinLength(3), MaxLength(500)]
    public string Title { get; set; }

    [NotNull]
    public string Content { get; set; }

    public ICollection<CreateCategoryRequest> Categories { get; set; } = new HashSet<CreateCategoryRequest>();

    public ICollection<CreateTagRequest> Tags { get; set; } = new HashSet<CreateTagRequest>();
}
