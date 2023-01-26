namespace NewHabr.Domain.Dto;

#nullable disable
public class RegistrationRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
    public int SecurityQuestionId { get; set; }
    public string SecurityQuestionAnswer { get; set; }
}
