using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(int categoryId)
        : base($"Category with id: '{categoryId}' not found")
    {
    }

    public CategoryNotFoundException(string name)
        : base($"Category with name: '{name}' not found")
    {
    }
}
