namespace NewHabr.Domain.Dto;

#nullable disable
public class RecoveryRequest
{
    public int TransactionId { get; set; }
    public string Answer { get; set; }
}
