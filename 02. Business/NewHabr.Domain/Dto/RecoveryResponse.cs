namespace NewHabr.Domain.Dto;

#nullable disable
public class RecoveryResponse
{
    public string Token { get; set; }
    public UserDto User { get; set; }
}
