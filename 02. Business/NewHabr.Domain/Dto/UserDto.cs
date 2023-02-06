namespace NewHabr.Domain.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    public ICollection<string> Roles { get; set; } = null!;
    public int? Age { get; set; }
    public string? Description { get; set; }
}
