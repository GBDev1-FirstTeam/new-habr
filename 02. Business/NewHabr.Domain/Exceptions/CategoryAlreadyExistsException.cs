using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class CategoryAlreadyExistsException : EntityAlreadyExistsException
{
    public CategoryAlreadyExistsException() : base(typeof(Category))
    {
    }
}
