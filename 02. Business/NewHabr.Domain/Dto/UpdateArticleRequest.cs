#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewHabr.Domain.Dto;

public class UpdateArticleRequest
{
    [NotNull, MinLength(3), MaxLength(500)]
    public string Title { get; set; }

    [NotNull]
    public string Content { get; set; }

    public CreateCategoryRequest[] Categories { get; set; } = Array.Empty<CreateCategoryRequest>();

    public CreateTagRequest[] Tags { get; set; } = Array.Empty<CreateTagRequest>();
}
