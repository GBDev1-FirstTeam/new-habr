using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class CategoryAlreadyExistsException : AlreadyExistsException
{
    public CategoryAlreadyExistsException(string name)
        : base($"Category '{name}' already exists")
    {
    }
}
