namespace NewHabr.Domain.Exceptions;

public class UserBannedException : Exception
{
    public UserBannedException() : base("User banned.")
    {
    }
}
