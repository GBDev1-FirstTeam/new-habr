#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class CreateArticleRequest
{
    [Required, MinLength(3), MaxLength(500)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    public ICollection<CreateCategoryRequest> Categories { get; set; } = new HashSet<CreateCategoryRequest>();

    public ICollection<CreateTagRequest> Tags { get; set; } = new HashSet<CreateTagRequest>();
}
