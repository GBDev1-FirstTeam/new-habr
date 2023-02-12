using System;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace NewHabr.Business.Services;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<UserRole> _roleManager;
    private readonly IMapper _mapper;


    public UserService(
        IMapper mapper,
        IRepositoryManager repositoryManager,
        UserManager<User> userManager,
        RoleManager<UserRole> roleManager)
    {
        _repositoryManager = repositoryManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }


    public async Task SetUserRolesAsync(Guid id, UserAssignRolesRequest request, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(id, false, cancellationToken);

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

        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task UpdateUserProfileAsync(Guid id, UserForManipulationDto userDataDto, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(id, true, cancellationToken);
        _mapper.Map(userDataDto, user);
        await _repositoryManager.SaveAsync();
    }

    public async Task<ICollection<UserArticleDto>> GetUserArticlesAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(id, false, cancellationToken);
        var articles = await _repositoryManager.ArticleRepository.GetUserArticlesAsync(id, false, cancellationToken);

        return _mapper.Map<List<UserArticleDto>>(articles);
    }

    public async Task<ICollection<UserNotificationDto>> GetUserNotificationsAsync(Guid id, bool unreadOnly, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(id, false, cancellationToken);
        var notifications = await _repositoryManager.NotificationRepository.GetUserNotificationsAsync(id, unreadOnly, false, cancellationToken);

        var notifDto = _mapper.Map<List<UserNotificationDto>>(notifications);
        return notifDto;
    }

    public async Task<ICollection<UserCommentDto>> GetUserCommentsAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(id, false, cancellationToken);

        var comments = await _repositoryManager.CommentRepository.GetUserCommentAsync(id, cancellationToken);

        var commentsDto = _mapper.Map<List<UserCommentDto>>(comments);
        return commentsDto;
    }

    public async Task<ICollection<LikedArticleDto>> GetUserLikedArticlesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(userId, false, cancellationToken);
        var articles = await _repositoryManager.ArticleRepository.GetUserLikedArticlesAsync(userId, cancellationToken);

        var articlesDto = _mapper.Map<ICollection<LikedArticleDto>>(articles);
        return articlesDto;
    }

    public async Task<ICollection<LikedCommentDto>> GetUserLikedCommentsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(userId, false, cancellationToken);
        var comments = await _repositoryManager.CommentRepository.GetUserLikedCommentsAsync(userId, cancellationToken);

        var commentsDto = _mapper.Map<ICollection<LikedCommentDto>>(comments);
        return commentsDto;
    }

    public async Task<ICollection<LikedUserDto>> GetUserLikedUsersAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(userId, false, cancellationToken);
        var users = await _repositoryManager.UserRepository.GetUserLikedUsersAsync(userId, cancellationToken);

        var usersDto = _mapper.Map<ICollection<LikedUserDto>>(users);
        return usersDto;
    }

    public async Task<UserProfileDto> GetUserInfoAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetUserAndCheckIfItExistsAsync(userId, false, cancellationToken);
        var userDto = _mapper.Map<UserProfileDto>(user);
        userDto.ReceivedLikes = await _repositoryManager
            .UserRepository
            .GetReceivedLikesCountAsync(userId, cancellationToken);

        return userDto;
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
}

