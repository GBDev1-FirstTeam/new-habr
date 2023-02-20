using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewHabr.Business.Extensions;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace NewHabr.Business.Services;
public class CommentService : ICommentService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;
    private readonly UserManager<User> _userManager;

    public CommentService(IRepositoryManager repositoryManager, IMapper mapper, INotificationService notificationService, UserManager<User> userManager)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _notificationService = notificationService;
        _userManager = userManager;
    }

    public async Task CreateAsync(Guid CreatorId, CommentCreateRequest data, CancellationToken cancellationToken)
    {
        await CheckIfUserNotBannedOrThrow(CreatorId, cancellationToken);

        var article = await _repositoryManager
            .ArticleRepository
            .GetByIdAsync(data.ArticleId, true, cancellationToken);

        if (article is null)
            throw new ArticleNotFoundException();

        var newComment = _mapper.Map<Comment>(data);
        newComment.UserId = CreatorId;

        var createDate = DateTimeOffset.UtcNow;
        newComment.CreatedAt = createDate;
        newComment.ModifiedAt = createDate;

        article.Comments.Add(newComment);
        await _repositoryManager.SaveAsync(cancellationToken);

        await CreateNotificationIfMentionSomeoneAsync(article, newComment, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleteComment = await _repositoryManager.CommentRepository.GetByIdAsync(id, true, cancellationToken);
        if (deleteComment is null)
        {
            throw new CommentNotFoundException();
        }

        _repositoryManager.CommentRepository.Delete(deleteComment);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CommentDto>> GetAllAsync(CancellationToken cancellationToken)
    {

        var comments = await _repositoryManager.CommentRepository.GetAllAsync(false, cancellationToken);
        return _mapper.Map<List<CommentDto>>(comments);
    }

    public async Task SetLikeAsync(Guid commentId, Guid userId, CancellationToken cancellationToken)
    {
        var comment = await _repositoryManager.CommentRepository.GetByIdWithLikesAsync(commentId, true, cancellationToken);

        if (comment is null)
        {
            throw new CommentNotFoundException();
        }

        if (comment.UserId == userId)
            return; // нечего лайкать свои комментарии

        await CheckIfUserNotBannedOrThrow(userId, cancellationToken);

        var user = await _repositoryManager.UserRepository.GetByIdAsync(userId, true, cancellationToken);

        comment.Likes.Add(user);

        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task UnsetLikeAsync(Guid commentId, Guid userId, CancellationToken cancellationToken)
    {
        var comment = await _repositoryManager.CommentRepository.GetByIdWithLikesAsync(commentId, true, cancellationToken);

        if (comment is null)
        {
            throw new CommentNotFoundException();
        }

        if (comment.UserId == userId)
            return; // нечего лайкать свои комментарии

        var user = await _repositoryManager.UserRepository.GetByIdAsync(userId, true, cancellationToken);
        comment.Likes.Remove(comment.Likes.FirstOrDefault(u => u.Id == user!.Id));
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task UpdateAsync(Guid commentId, Guid modifierId, CommentUpdateRequest updatedComment, CancellationToken cancellationToken)
    {
        await CheckIfUserNotBannedOrThrow(modifierId, cancellationToken);

        var comment = await _repositoryManager.CommentRepository.GetByIdAsync(commentId, true, cancellationToken: cancellationToken);

        if (comment is null)
        {
            throw new CommentNotFoundException();
        }

        if (comment.UserId != modifierId)
        {
            throw new UnauthorizedAccessException();
        }

        _mapper.Map(updatedComment, comment);
        comment.ModifiedAt = DateTimeOffset.UtcNow;

        await _repositoryManager.SaveAsync(cancellationToken);
    }



    private async Task CheckIfUserNotBannedOrThrow(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _repositoryManager.UserRepository.GetByIdAsync(userId, true, cancellationToken);

        if (user!.Banned)
            throw new UserBannedException(user.BannedAt!.Value);
    }

    private async Task CreateNotificationIfMentionSomeoneAsync(Article article, Comment comment, CancellationToken cancellationToken)
    {
        await CreateNotificationOnModeratorMention(article, comment, cancellationToken);
        await CreateNotificationOnUserMention(article, comment, cancellationToken);
    }

    private async Task CreateNotificationOnModeratorMention(Article article, Comment comment, CancellationToken cancellationToken)
    {
        string anchor = "@moderator";
        if (!comment.Text.StartsWith(anchor, StringComparison.OrdinalIgnoreCase))
            return;

        var users = await _userManager
            .GetUsersInRoleAsync(UserRoles.Moderator.ToString());

        if (users.Count == 0)
            return;

        var text = new StringBuilder();
        text.AppendLine($"Модератора желают видеть в комментариях к статье '{article.Title}'");
        text.AppendLine(comment.Text.Substring(anchor.Length).Trim());
        var notification = new NotificationCreateRequest
        {
            Text = text.ToString()
        };

        await _notificationService
            .CreateAsync(notification, users, cancellationToken);
    }

    private async Task CreateNotificationOnUserMention(Article article, Comment comment, CancellationToken cancellationToken)
    {
        var usernames = comment
            .Text
            .FindMentionedUsers();

        if (usernames.Count == 0)
            return;

        var users = await _repositoryManager
            .UserRepository
            .GetUsersByLoginAsync(usernames, true, cancellationToken);

        if (users.Count == 0)
            return;

        var notification = new NotificationCreateRequest
        {
            Text = $"Вас упомянули в комментарии к статье '{article.Title}'"
        };
        await _notificationService.CreateAsync(notification, users, cancellationToken);
    }
}
