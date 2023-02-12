#nullable disable

namespace NewHabr.Domain.Dto;

public class UserAssignRolesResponse
{
    public Guid Id { get; set; }
    public ICollection<string> Roles { get; set; }
}
