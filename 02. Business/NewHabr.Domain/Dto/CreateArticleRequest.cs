#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class CreateArticleRequest
{
    [Required, MinLength(3), MaxLength(500)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    public UpdateCategoryRequest[] Categories { get; set; } = Array.Empty<UpdateCategoryRequest>();

    public CreateTagRequest[] Tags { get; set; } = Array.Empty<CreateTagRequest>();
}
