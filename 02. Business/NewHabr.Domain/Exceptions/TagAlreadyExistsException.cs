using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class TagAlreadyExistsException : EntityAlreadyExistsException
{
    public TagAlreadyExistsException() : base(typeof(Tag))
    {
    }
}
