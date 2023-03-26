#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class Notification : BaseEntity<Guid>
{
    [Required]
    public ICollection<User> Users { get; set; }

    [Required]
    public string Text { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public bool IsRead { get; set; }
}
