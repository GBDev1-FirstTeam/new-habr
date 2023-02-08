using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;
using static NewHabr.Domain.Constants;

namespace NewHabr.Domain.Contracts.Services;
public interface ICommentService
{
    /// <summary>
    /// Creates new Comment class and adds it to the database
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>    
    Task CreateAsync(CreateCommentRequest data, CancellationToken cancellationToken = default);
    /// <summary>
    /// Updates an existing in database Comment class
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="CommentNotFoundException"></exception>
    Task UpdateAsync(Guid id,
        UpdateCommentRequest updatedComment,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// Deletes an existing in database Comment class 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="CommentNotFoundException"></exception>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Returns a list of all existing in database Comments classes 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<CommentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Returns a list of existing in database Comments classes matched to User.Id
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<CommentDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Returns a list of existing in database Comments classes matched to Article.Id
    /// </summary>
    /// <param name="articleId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<CommentDto>> GetByArticleIdAsync(Guid articleId, CancellationToken cancellationToken = default);
}
