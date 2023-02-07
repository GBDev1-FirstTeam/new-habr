using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public abstract class EntityNotFoundException : Exception
{
    protected EntityNotFoundException(Type type) : base($"Entity type of {type.Name} not found.")
    {
    }
}
