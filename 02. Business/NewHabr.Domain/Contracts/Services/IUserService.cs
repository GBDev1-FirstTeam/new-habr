using System;
using NewHabr.Domain.Dto;

namespace NewHabr.Domain.Contracts.Services;

public interface IUserService
{
    Task<UserProfileDto> UpdateUserProfile(Guid id, UserForManipulationDto userDto, CancellationToken cancellation);
    Task SetUserRoles(Guid id, UserAssignRolesRequest request, CancellationToken cancellationToken);
    Task<UserAssignRolesResponse> GetUserRoles(Guid id, CancellationToken cancellationToken);
    Task SetBanOnUser(Guid id, UserBanDto userBanDto, CancellationToken cancellationToken);
}

