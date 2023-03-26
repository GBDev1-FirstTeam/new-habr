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
    Task<CommentDto> CreateAsync(Guid creatorId, CommentCreateRequest data, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing in database Comment class
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="CommentNotFoundException"></exception>
    Task UpdateAsync(Guid commentId, CommentUpdateRequest updatedComment, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes an existing in database Comment class 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="CommentNotFoundException"></exception>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Returns a list of all existing in database Comments classes 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<CommentDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<CommentDto> GetByIdAsync(Guid commentId, CancellationToken cancellationToken);

    ///// <summary>
    ///// Returns a list of existing in database Comments classes matched to User.Id
    ///// </summary>
    ///// <param name="userId"></param>
    ///// <param name="cancellationToken"></param>
    ///// <returns></returns>
    //Task<IReadOnlyCollection<CommentDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    ///// <summary>
    ///// Returns a list of existing in database Comments classes matched to Article.Id
    ///// </summary>
    ///// <param name="articleId"></param>
    ///// <param name="cancellationToken"></param>
    ///// <returns></returns>
    //Task<IReadOnlyCollection<CommentDto>> GetByArticleIdAsync(Guid articleId, CancellationToken cancellationToken);

    /// <summary>
    /// Sets 'Like' mark at comment
    /// </summary>
    Task SetLikeAsync(Guid commentId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Unsets 'Like' mark at comment
    /// </summary>
    Task UnsetLikeAsync(Guid commentId, Guid userId, CancellationToken cancellationToken);

    Task<bool> IsAuthor(Guid commentId, Guid userId, CancellationToken cancellationToken);
}
