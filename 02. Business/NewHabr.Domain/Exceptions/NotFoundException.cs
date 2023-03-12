using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public abstract class NotFoundException : Exception
{
    protected NotFoundException(string message) : base(message)
    {
    }
}
