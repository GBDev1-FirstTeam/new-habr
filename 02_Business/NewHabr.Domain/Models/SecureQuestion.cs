#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class SecureQuestion : BaseEntity<int>
{
    [Required]
    public string Question { get; set; }
}
