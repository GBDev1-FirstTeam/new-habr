using System;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts.Services;

public interface IUserService
{
    /// <summary>
    /// Get list of authored articles
    /// </summary>
    /// <param name="id">user id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ICollection<UserArticleDto>> GetUserArticlesAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Get User profile info
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UserProfileDto> GetUserInfoAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Update user's profile
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userDto"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    Task UpdateUserProfileAsync(Guid id, UserForManipulationDto userDto, CancellationToken cancellation);

    /// <summary>
    /// Set list of user's roles
    /// </summary>
    /// <param name="id">user id</param>
    /// <param name="request">object with list of roles</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SetUserRolesAsync(Guid id, UserAssignRolesRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of user's roles
    /// </summary>
    /// <param name="id">user id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UserAssignRolesResponse> GetUserRolesAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Set BAN on user 
    /// </summary>
    /// <param name="userId">banned user id</param>
    /// <returns></returns>
    Task SetBanOnUserAsync(Guid id, UserBanDto userBanDto, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of user's comments 
    /// </summary>
    /// <param name="userId">user id who is comment author</param>
    /// <returns></returns>
    Task<ICollection<UserCommentDto>> GetUserCommentsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of Notifications
    /// </summary>
    /// <param name="userId">user id who got a notification</param>
    /// <returns></returns>
    Task<ICollection<NotificationDto>> GetUserNotificationsAsync(Guid id, bool unreadOnly, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of articles where 'like' was set
    /// </summary>
    /// <param name="userId">'like' author</param>
    /// <returns></returns>
    Task<ICollection<LikedArticleDto>> GetUserLikedArticlesAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of comments where 'like' was set
    /// </summary>
    /// <param name="userId">'like' author</param>
    /// <returns></returns>
    Task<ICollection<LikedCommentDto>> GetUserLikedCommentsAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of authors where 'like' was set
    /// </summary>
    /// <param name="userId">'like' author</param>
    /// <returns></returns>
    Task<ICollection<LikedUserDto>> GetUserLikedUsersAsync(Guid userId, CancellationToken cancellationToken);
}

