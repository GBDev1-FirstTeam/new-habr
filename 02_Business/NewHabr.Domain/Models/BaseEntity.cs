using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public abstract class BaseEntity<TId> : IEntity<TId> where TId : struct
{
    [Key]
    public TId Id { get; set; }
    public bool Deleted { get; set; }
}
