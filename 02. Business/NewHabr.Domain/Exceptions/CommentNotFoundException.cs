using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;
public class CommentNotFoundException : NotFoundException
{
    public CommentNotFoundException(Guid commentId)
        : base($"Comment with id: '{commentId}' not found")
    {
    }
}
