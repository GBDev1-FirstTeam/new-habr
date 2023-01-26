namespace NewHabr.Domain.Dto;

public class RegistrationRequest
{
    public string UserName { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int SecurityQuestionId { get; set; }
    public string SecurityQuestionAnswer { get; set; } = null!;
}
