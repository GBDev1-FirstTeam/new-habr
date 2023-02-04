using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public abstract class EntityAlreadyExistsException<TEntity, TId> : Exception
    where TEntity : IEntity<TId>
    where TId : struct
{
    public EntityAlreadyExistsException() : base($"Entity type of {nameof(TEntity)} already exists.")
    {
    }
}
