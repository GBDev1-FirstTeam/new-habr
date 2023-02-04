using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class CategoryNotFoundException : EntityNotFoundException<Category, int>
{
}
