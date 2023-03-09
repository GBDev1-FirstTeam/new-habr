using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewHabr.Domain;
using NewHabr.Domain.ConfigurationModels;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;
using NewHabr.Domain.ServiceModels;

namespace NewHabr.Business.Services;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<UserRole> _roleManager;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;


    public UserService(
        IOptions<AppSettings> appSettings,
        IMapper mapper,
        IRepositoryManager repositoryManager,
        UserManager<User> userManager,
        RoleManager<UserRole> roleManager,
        ILogger<UserService> logger)
    {
        _repositoryManager = repositoryManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _mapper = mapper;
        _appSettings = appSettings.Value;
    }


    public async Task SetUserRolesAsync(Guid id, UserAssignRolesRequest request, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(id, true, cancellationToken);

        await CheckIfUserRolesExist(request.Roles);

        var rolesAlreadyIn = await GetUserRoles(user);
        var result = await _userManager.AddToRolesAsync(user, request.Roles.Except(rolesAlreadyIn));

        rolesAlreadyIn = await GetUserRoles(user);
        result = await _userManager.RemoveFromRolesAsync(user, rolesAlreadyIn.Except(request.Roles));
    }

    public async Task<UserAssignRolesResponse> GetUserRolesAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(id, false, cancellationToken);
        return new UserAssignRolesResponse { Id = user.Id, Roles = await GetUserRoles(user) };
    }

    public async Task SetBanOnUserAsync(Guid id, UserBanDto userBanDto, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(id, true, cancellationToken);
        user.Banned = true;
        user.BannedAt = DateTimeOffset.UtcNow;
        user.BanReason = userBanDto.BanReason;
        user.BanExpiratonDate = user.BannedAt.Value.AddDays(_appSettings.UserBanExpiresInDays);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task UpdateUserProfileAsync(Guid id, UserForManipulationDto userDataDto, CancellationToken cancellationToken)
    {
        if (userDataDto.BirthDay.HasValue && (userDataDto.BirthDay.Value < -62135596800000 || userDataDto.BirthDay.Value > 253402300799999))
            throw new ArgumentException("DateTime out of range", nameof(userDataDto));

        var user = await GetUserAndCheckIfItExistsAsync(id, true, cancellationToken);
        _mapper.Map(userDataDto, user);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task<ArticlesGetResponse> GetUserArticlesAsync(Guid userId, Guid whoAskingId, ArticleQueryParametersDto queryParamsDto, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(userId, false, cancellationToken);
        var queryParams = _mapper.Map<ArticleQueryParameters>(queryParamsDto);

        PagedList<ArticleModel> articles;

        if (userId.Equals(whoAskingId))
        {
            articles = await _repositoryManager
                .ArticleRepository
                .GetAllByAuthorIdAsync(userId, false, queryParams, cancellationToken);
        }
        else
        {
            articles = await _repositoryManager
                .ArticleRepository
                .GetByAuthorIdAsync(userId, whoAskingId, false, queryParams, cancellationToken);
        }

        return new ArticlesGetResponse { Articles = _mapper.Map<ICollection<ArticleDto>>(articles), Metadata = articles.Metadata };
    }

    public async Task<ICollection<NotificationDto>> GetUserNotificationsAsync(Guid id, bool unreadOnly, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(id, false, cancellationToken);
        var notifications = await _repositoryManager.NotificationRepository.GetUserNotificationsAsync(id, unreadOnly, false, cancellationToken);

        var notifDto = _mapper.Map<List<NotificationDto>>(notifications);
        return notifDto;
    }

    public async Task<ICollection<UserCommentDto>> GetUserCommentsAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(id, false, cancellationToken);

        var comments = await _repositoryManager.CommentRepository.GetUserCommentAsync(id, false, cancellationToken);

        var commentsDto = _mapper.Map<List<UserCommentDto>>(comments);
        return commentsDto;
    }

    public async Task<ArticlesGetResponse> GetUserLikedArticlesAsync(Guid userId, ArticleQueryParametersDto queryParamsDto, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(userId, false, cancellationToken);
        var queryParams = _mapper.Map<ArticleQueryParameters>(queryParamsDto);
        var articles = await _repositoryManager
            .ArticleRepository
            .GetUserLikedArticlesAsync(userId, Guid.Empty, false, queryParams, cancellationToken);

        return new ArticlesGetResponse { Articles = _mapper.Map<ICollection<ArticleDto>>(articles), Metadata = articles.Metadata };
    }

    public async Task<ICollection<LikedCommentDto>> GetUserLikedCommentsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(userId, false, cancellationToken);
        var comments = await _repositoryManager.CommentRepository.GetUserLikedCommentsAsync(userId, false, cancellationToken);

        var commentsDto = _mapper.Map<ICollection<LikedCommentDto>>(comments);
        return commentsDto;
    }

    public async Task<ICollection<LikedUserDto>> GetUserLikedUsersAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(userId, false, cancellationToken);
        var users = await _repositoryManager.UserRepository.GetUserLikedUsersAsync(userId, false, cancellationToken);

        var usersDto = _mapper.Map<ICollection<LikedUserDto>>(users);
        return usersDto;
    }

    public async Task<UserProfileDto> GetUserInfoAsync(Guid userId, Guid authUserId, CancellationToken cancellationToken)
    {
        var user = await _repositoryManager
            .UserRepository
            .GetUserInfoAsync(userId, authUserId, cancellationToken);

        if (user is null)
            throw new UserNotFoundException();

        var userDto = _mapper.Map<UserProfileDto>(user);
        userDto.ReceivedLikes = await _repositoryManager
            .UserRepository
            .GetReceivedLikesCountAsync(userId, false, cancellationToken);

        return userDto;
    }

    public async Task SetLikeAsync(Guid userId, Guid authUserId, CancellationToken cancellationToken)
    {
        if (userId == authUserId)
            return;

        await CheckIfUserNotBannedOrThrow(authUserId, cancellationToken);

        var likeReceiverUser = await _repositoryManager
            .UserRepository
            .GetByIdWithLikesAsync(userId, true, cancellationToken);

        if (likeReceiverUser is null)
            throw new UserNotFoundException();

        var likeSenderUser = await _repositoryManager
            .UserRepository
            .GetByIdAsync(authUserId, true, cancellationToken);

        likeReceiverUser.ReceivedLikes.Add(likeSenderUser);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task UnsetLikeAsync(Guid userId, Guid authUserId, CancellationToken cancellationToken)
    {
        if (userId == authUserId)
            return;

        var likeReceiverUser = await _repositoryManager
            .UserRepository
            .GetByIdWithLikesAsync(userId, true, cancellationToken);

        if (likeReceiverUser is null)
            throw new UserNotFoundException();

        var likeSenderUser = await _repositoryManager
            .UserRepository
            .GetByIdAsync(authUserId, true, cancellationToken);

        likeReceiverUser.ReceivedLikes.Remove(likeReceiverUser.ReceivedLikes.FirstOrDefault(lsu => lsu.Id == likeSenderUser!.Id));
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task<IdentityResult> ChangeUsername(Guid userId, UsernameChangeRequest request, CancellationToken cancellationToken)
    { // при смене юзернейма, claim username в токене останется прежним
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            throw new UserNotFoundException();

        var result = await _userManager.SetUserNameAsync(user, request.Username);
        return result;
    }

    public async Task<IdentityResult> ChangePassword(Guid userId, UserPasswordChangeRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            throw new UserNotFoundException();

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        return result;
    }

    public async Task<ICollection<UserProfileDto>> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _repositoryManager
            .UserRepository
            .GetAllAsync(false, cancellationToken);

        List<UserProfileDto> userProfileDtos = new List<UserProfileDto>();

        foreach (var user in users)
        {
            var dto = _mapper.Map<UserProfileDto>(user);
            dto.Roles = await _userManager.GetRolesAsync(user);
            userProfileDtos.Add(dto);
        }
        return userProfileDtos;
    }

    public async Task UnBanUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(userId, true, cancellationToken);

        if (!user.Banned)
            return;

        user.Banned = false;
        user.BannedAt = null;
        user.BanReason = null;
        user.BanExpiratonDate = null;
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task AutomaticUnbanUserAsync(CancellationToken cancellationToken)
    {
        var bannedUsers = await _repositoryManager
            .UserRepository
            .GetBannedUsersReadyToBeUnbannedAsync(true, cancellationToken);

        if (bannedUsers.Count == 0)
            return;

        foreach (var user in bannedUsers)
        {
            _logger.LogInformation($"User '{user.UserName}'(ID: {user.Id}) was unbanned automatically. User was banned at '{user.BannedAt}' for reason: '{user.BanReason}' till '{user.BanExpiratonDate}'");
            user.Banned = false;
            user.BannedAt = null;
            user.BanExpiratonDate = null;
            user.BanReason = null;
        }

        await _repositoryManager
            .SaveAsync(cancellationToken);
    }



    private async Task<User> GetUserAndCheckIfItExistsAsync(Guid id, bool trackChanges, CancellationToken cancellationToken)
    {
        var user = await _repositoryManager.UserRepository.GetByIdAsync(id, trackChanges, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        return user;
    }

    private async Task CheckIfUserRolesExist(ICollection<string> roles)
    {
        ICollection<string> rolesNotExist = new List<string>(roles.Count);
        foreach (var role in roles)
        {
            bool exist = await _roleManager.RoleExistsAsync(role);
            if (!exist)
                rolesNotExist.Add(role);
        }
        if (rolesNotExist.Count > 0)
            throw new ArgumentException($"Specified roles could not be found: {string.Join(", ", rolesNotExist)}");
    }

    private async Task<ICollection<string>> GetUserRoles(User user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    private async Task CheckIfUserNotBannedOrThrow(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _repositoryManager.UserRepository.GetByIdAsync(userId, true, cancellationToken);

        if (user!.Banned)
            throw new UserBannedException(user.BannedAt!.Value);
    }

    public async Task<RecoveryResponse> ForgotPasswordAsync(RecoveryRequest recoveryRequest, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(recoveryRequest.UserName);

        if (user is null)
        {
            throw new UserNotFoundException();
        }
        if (user.SecureQuestionId != recoveryRequest.SecureQuestionId || user.SecureAnswer != recoveryRequest.Answer)
        {
            await _userManager.AccessFailedAsync(user);
            throw new UnauthorizedAccessException();
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var userDto = _mapper.Map<UserDto>(user);

        return new RecoveryResponse()
        {
            Token = token,
            User = userDto
        };
    }

    public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(resetPasswordRequest.UserName);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var result = await _userManager.ResetPasswordAsync(user, resetPasswordRequest.Token, resetPasswordRequest.NewPassword);
        return result;
    }
}
