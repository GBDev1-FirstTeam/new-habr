namespace NewHabr.Domain.Dto;

#nullable disable
public class RegistrationResponse
{
    public string Token { get; set; }
    public UserDto User { get; set; }
}
