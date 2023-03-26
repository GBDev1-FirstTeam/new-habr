using System;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions
{
    public class SecureQuestionNotFoundException : NotFoundException
    {
        public SecureQuestionNotFoundException(int id)
            : base($"SecureQuestion with id: '{id}' not found")
        {
        }
    }
}

