#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class Category : BaseEntity<int>
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public ICollection<Article> Articles { get; set; }
}
