using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class TagNotFoundException : EntityNotFoundException<Tag, int>
{
}
