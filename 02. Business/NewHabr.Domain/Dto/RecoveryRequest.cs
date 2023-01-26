namespace NewHabr.Domain.Dto;

public class RecoveryRequest
{
    public int TransactionId { get; set; }
    public string Answer { get; set; } = null!;
}
