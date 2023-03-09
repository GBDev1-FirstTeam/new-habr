using System;
using Microsoft.AspNetCore.Identity;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts.Services;

public interface IUserService
{
    /// <summary>
    /// Get All Users
    /// </summary>
    Task<ICollection<UserProfileDto>> GetUsers(CancellationToken cancellationToken);
    /// <summary>
    /// Get list of authored articles
    /// </summary>
    Task<ArticlesGetResponse> GetUserArticlesAsync(Guid userId, Guid whoAskingId, ArticleQueryParametersDto queryParamsDto, CancellationToken cancellationToken);

    /// <summary>
    /// Get User profile info
    /// </summary>
    Task<UserProfileDto> GetUserInfoAsync(Guid userId, Guid authUserId, CancellationToken cancellationToken);

    /// <summary>
    /// Update user's profile
    /// </summary>
    Task UpdateUserProfileAsync(Guid id, UserForManipulationDto userDto, CancellationToken cancellation);

    /// <summary>
    /// Set list of user's roles
    /// </summary>
    /// <param name="id">user id</param>
    /// <param name="request">object with list of roles</param>
    Task SetUserRolesAsync(Guid id, UserAssignRolesRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of user's roles
    /// </summary>
    /// <param name="id">user id</param>
    Task<UserAssignRolesResponse> GetUserRolesAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Set BAN on user 
    /// </summary>
    /// <param name="userId">banned user id</param>
    Task SetBanOnUserAsync(Guid id, UserBanDto userBanDto, CancellationToken cancellationToken);

    /// <summary>
    /// Reset ban flag
    /// </summary>
    Task UnBanUserAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Finds users banned where ban expiration time reached and unban them
    /// </summary>
    Task AutomaticUnbanUserAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Get list of user's comments 
    /// </summary>
    /// <param name="userId">user id who is comment author</param>
    Task<ICollection<UserCommentDto>> GetUserCommentsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of Notifications
    /// </summary>
    /// <param name="userId">user id who got a notification</param>
    Task<ICollection<NotificationDto>> GetUserNotificationsAsync(Guid id, bool unreadOnly, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of articles where 'like' was set
    /// </summary>
    /// <param name="userId">'like' author</param>
    Task<ArticlesGetResponse> GetUserLikedArticlesAsync(Guid userId, ArticleQueryParametersDto queryParamsDto, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of comments where 'like' was set
    /// </summary>
    /// <param name="userId">'like' author</param>
    Task<ICollection<LikedCommentDto>> GetUserLikedCommentsAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of authors where 'like' was set
    /// </summary>
    /// <param name="userId">'like' author</param>
    Task<ICollection<LikedUserDto>> GetUserLikedUsersAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Sets 'Like' mark at user
    /// </summary>
    Task SetLikeAsync(Guid userId, Guid authUserId, CancellationToken cancellationToken);

    /// <summary>
    /// Unsets 'Like' mark at user
    /// </summary>
    Task UnsetLikeAsync(Guid userId, Guid authUserId, CancellationToken cancellationToken);

    /// <summary>
    /// Change username, username must be unique
    /// </summary>
    Task<IdentityResult> ChangeUsername(Guid userId, UsernameChangeRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Change password
    /// </summary>
    Task<IdentityResult> ChangePassword(Guid userId, UserPasswordChangeRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Request password reset token
    /// </summary>
    Task<RecoveryResponse> ForgotPasswordAsync(RecoveryRequest recoveryRequest, CancellationToken cancellationToken);

    /// <summary>
    /// Reset password using password reset token
    /// </summary>
    Task<IdentityResult> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken);
}

