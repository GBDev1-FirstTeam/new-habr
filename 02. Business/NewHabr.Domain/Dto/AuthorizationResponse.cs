namespace NewHabr.Domain.Dto;

#nullable disable
public class AuthorizationResponse
{
    public string Token { get; set; }
    public UserDto User { get; set; }
}
