namespace NewHabr.Domain.Models;

public class SecureQuestion : BaseEntity<int>
{
    public string Question { get; set; } = string.Empty;
}
