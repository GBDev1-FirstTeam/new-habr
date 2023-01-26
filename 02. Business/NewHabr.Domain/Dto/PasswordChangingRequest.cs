namespace NewHabr.Domain.Dto;

#nullable disable
public class PasswordChangingRequest
{
    public Guid UserId { get; set; }
    public string LastPassword { get; set; }
    public string NewPassword { get; set; }
}
