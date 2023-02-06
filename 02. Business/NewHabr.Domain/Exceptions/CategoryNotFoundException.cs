using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class CategoryNotFoundException : EntityNotFoundException
{
    public CategoryNotFoundException() : base(typeof(Category))
    {
    }
}
