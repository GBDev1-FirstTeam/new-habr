namespace NewHabr.Domain.Dto;

public class RegistrationResponse
{
    public string Token { get; set; } = null!;
    public UserDto User { get; set; } = null!;
}
