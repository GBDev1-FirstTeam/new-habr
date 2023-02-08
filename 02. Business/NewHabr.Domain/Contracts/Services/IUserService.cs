using System;
using NewHabr.Domain.Dto;

namespace NewHabr.Domain.Contracts.Services;

public interface IUserService
{
    Task<ICollection<UserArticleDto>> GetUserArticlesAsync(Guid id, CancellationToken cancellationToken);
    Task<UserProfileDto> UpdateUserProfileAsync(Guid id, UserForManipulationDto userDto, CancellationToken cancellation);
    Task SetUserRolesAsync(Guid id, UserAssignRolesRequest request, CancellationToken cancellationToken);
    Task<UserAssignRolesResponse> GetUserRolesAsync(Guid id, CancellationToken cancellationToken);
    Task SetBanOnUserAsync(Guid id, UserBanDto userBanDto, CancellationToken cancellationToken);
    Task<ICollection<UserNotificationDto>> GetUserNotificationsAsync(Guid id, bool unreadOnly, CancellationToken cancellationToken);
}

