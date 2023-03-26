using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class TagAlreadyExistsException : AlreadyExistsException
{
    public TagAlreadyExistsException(string name)
        : base($"Tag '{name}' already exists")
    {
    }
}
