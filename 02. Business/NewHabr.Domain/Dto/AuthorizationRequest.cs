namespace NewHabr.Domain.Dto;

public class AuthorizationRequest
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}
