namespace NewHabr.Domain.Models;

public abstract class BaseEntity<TId> where TId : struct
{
    public TId Id { get; set; }
}
