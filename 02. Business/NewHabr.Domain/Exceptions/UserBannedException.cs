using System;
namespace NewHabr.Domain.Exceptions;

public class UserBannedException : Exception
{
    public UserBannedException(DateTimeOffset bannedAt) : base($"User has been banned at {bannedAt}")
    {
    }
}

