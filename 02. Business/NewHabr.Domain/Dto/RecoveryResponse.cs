namespace NewHabr.Domain.Dto;

public class RecoveryResponse
{
    public string Token { get; set; } = null!;
    public UserDto User { get; set; } = null!;
}
