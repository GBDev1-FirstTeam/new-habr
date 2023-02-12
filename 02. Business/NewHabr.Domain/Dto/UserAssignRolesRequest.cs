#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class UserAssignRolesRequest
{
    [MinLength(1)]
    public ICollection<string> Roles { get; set; }
}
