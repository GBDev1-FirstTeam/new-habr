using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class TagNotFoundException : NotFoundException
{
    public TagNotFoundException(int tagId)
        : base($"Tag with id: '{tagId}' not found")
    {
    }
}
