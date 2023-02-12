#nullable disable

using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class LikedArticleDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public ICollection<CategoryDto> Categories { get; set; }

    public ICollection<TagDto> Tags { get; set; }
}
