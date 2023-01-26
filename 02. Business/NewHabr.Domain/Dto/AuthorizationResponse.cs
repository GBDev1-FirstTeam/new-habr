namespace NewHabr.Domain.Dto;

public class AuthorizationResponse
{
    public string Token { get; set; } = null!;
    public UserDto User { get; set; } = null!;
}
