namespace NewHabr.Domain.Dto;

#nullable disable
public class UserDto
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string Role { get; set; }
    public int Age { get; set; }
    public string Description { get; set; }
}
