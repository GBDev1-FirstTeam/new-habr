using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class Category : BaseEntity<int>
{
    [Required]
    public string Text { get; set; } = string.Empty;

    public ICollection<Article> Articles { get; set; } = null!;
}
