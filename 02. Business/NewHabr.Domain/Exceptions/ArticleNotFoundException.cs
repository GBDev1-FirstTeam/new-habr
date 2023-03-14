using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class ArticleNotFoundException : NotFoundException
{
    public ArticleNotFoundException(Guid articleId)
        : base($"Article with id: '{articleId}' not found")
    {
    }
}
