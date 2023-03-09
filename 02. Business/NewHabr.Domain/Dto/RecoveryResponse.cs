#nullable disable
namespace NewHabr.Domain.Dto;

public class RecoveryResponse
{
    public string Token { get; set; }

    public UserDto User { get; set; }
}
