namespace NewHabr.Domain.Exceptions;

public abstract class EntityAlreadyExistsException : Exception
{
    protected EntityAlreadyExistsException(Type type) : base($"Entity type of {type.Name} already exists.")
    {
    }
}
