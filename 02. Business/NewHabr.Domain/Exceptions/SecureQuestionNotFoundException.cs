using System;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions
{
    public class SecureQuestionNotFoundException : EntityNotFoundException
    {
        public SecureQuestionNotFoundException() : base(typeof(SecureQuestion))
        {
        }
    }
}

