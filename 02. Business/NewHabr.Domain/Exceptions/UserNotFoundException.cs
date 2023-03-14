using System;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(Guid userId)
        : base($"User with id: '{userId}' not found")
    {
    }

    public UserNotFoundException(string name)
        : base($"User with name: '{name}' not found")
    {
    }
}

