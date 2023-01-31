using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewHabr.DAL.EF;

namespace NewHabr.DAL.Repository;
public interface IRepositoryManager
{    
    public IArticleRepository ArticleRepository { get; }
    public ICommentRepository CommentRepository { get; }
    public IUserRepository UserRepository { get; }
    public Task SaveAsync(CancellationToken cancellationToken = default);
}
