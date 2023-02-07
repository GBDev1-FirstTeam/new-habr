using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;
public class CommentNotFoundException : EntityNotFoundException
{
    public CommentNotFoundException() : base(typeof(Comment))
    {
    }
}
