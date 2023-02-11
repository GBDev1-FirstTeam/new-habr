namespace NewHabr.Domain.Exceptions;

public class ArticleIsNotApprovedException : Exception
{
    public ArticleIsNotApprovedException() : base($"Article is not approve.")
    {
    }
}
