#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class CreateArticleRequest
{
    [Required]
    public Guid UserId { get; set; }

    [MinLength(3), MaxLength(500)]
    public string Title { get; set; }

    public string Content { get; set; }

    public ICollection<CreateCategoryRequest> Categories { get; set; } = new HashSet<CreateCategoryRequest>();

    public ICollection<CreateTagRequest> Tags { get; set; } = new HashSet<CreateTagRequest>();
}
