using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class CategoryAlreadyExistsException : EntityAlreadyExistsException<Category, int>
{
}
