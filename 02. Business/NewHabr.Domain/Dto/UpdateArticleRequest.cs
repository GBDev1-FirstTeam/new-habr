#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class UpdateArticleRequest
{
    [Required, MinLength(3), MaxLength(500)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    public CreateCategoryRequest[] Categories { get; set; } = Array.Empty<CreateCategoryRequest>();

    public CreateTagRequest[] Tags { get; set; } = Array.Empty<CreateTagRequest>();
}
