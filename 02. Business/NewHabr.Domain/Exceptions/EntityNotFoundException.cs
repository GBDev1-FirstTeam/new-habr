using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public abstract class EntityNotFoundException<TEntity, TId> : Exception
    where TEntity : IEntity<TId>
    where TId : struct
{
    public EntityNotFoundException() : base($"Entity type of {nameof(TEntity)} not found.")
    {
    }
}
