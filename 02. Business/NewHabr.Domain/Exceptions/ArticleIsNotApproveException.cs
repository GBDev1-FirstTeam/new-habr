namespace NewHabr.Domain.Exceptions;

public class ArticleIsNotApproveException : Exception
{
    public ArticleIsNotApproveException() : base($"Article is not approve.")
    {
    }
}
