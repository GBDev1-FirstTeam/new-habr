using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class TagNotFoundException : EntityNotFoundException
{
    public TagNotFoundException() : base(typeof(Tag))
    {
    }
}
