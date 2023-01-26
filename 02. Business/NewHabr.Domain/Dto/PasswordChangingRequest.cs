namespace NewHabr.Domain.Dto;

public class PasswordChangingRequest
{
    public Guid UserId { get; set; }
    public string LastPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
