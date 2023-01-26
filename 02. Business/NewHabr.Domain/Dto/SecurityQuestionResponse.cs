namespace NewHabr.Domain.Dto;

public class SecurityQuestionResponse
{
    public int TransactionId { get; set; }
    public string SecurityQuestion { get; set; } = null!;
}
