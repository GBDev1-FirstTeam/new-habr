using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Exceptions;
public class CommentNotFoundException : EntityNotFoundException<Comment, Guid>
{
}
