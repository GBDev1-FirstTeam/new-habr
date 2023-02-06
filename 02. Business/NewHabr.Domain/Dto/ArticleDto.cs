#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewHabr.Domain.Dto;

public class ArticleDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [NotNull, MinLength(3), MaxLength(500)]
    public string Title { get; set; }

    public CategoryDto[] Categories { get; set; }

    public TagDto[] Tags { get; set; }

    [NotNull]
    public string Content { get; set; }

    public long CreatedAt { get; set; }

    public long ModifiedAt { get; set; }

    public long PublishedAt { get; set; }
}
