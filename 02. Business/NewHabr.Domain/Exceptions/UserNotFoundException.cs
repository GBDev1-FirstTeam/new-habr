using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class UserNotFoundException : EntityNotFoundException
{
    public UserNotFoundException() : base(typeof(User))
    {
    }
}
