using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;

public class ArticleNotFoundException : EntityNotFoundException<Article, Guid>
{
}
