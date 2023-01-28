#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class Tag : BaseEntity<int>
{
    [Required, MaxLength(50)]
    public string Name { get; set; }

    public ICollection<Article> Articles { get; set; }
}
