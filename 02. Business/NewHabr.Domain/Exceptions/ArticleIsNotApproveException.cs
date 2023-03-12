namespace NewHabr.Domain.Exceptions;

public class ArticleNotApprovedException : Exception
{
    public ArticleNotApprovedException(Guid articleId)
        : base($"Article with id '{articleId}' is not approved.")
    {
    }
}
