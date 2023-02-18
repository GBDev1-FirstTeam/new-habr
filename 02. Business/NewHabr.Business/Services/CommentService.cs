﻿using AutoMapper;
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

    public CommentService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task CreateAsync(Guid CreatorId, CommentCreateRequest data, CancellationToken cancellationToken = default)
    {
        await CheckIfUserNotBannedOrThrow(CreatorId, cancellationToken);

        var newComment = _mapper.Map<Comment>(data);
        newComment.UserId = CreatorId;

        var createDate = DateTimeOffset.UtcNow;
        newComment.CreatedAt = createDate;
        newComment.ModifiedAt = createDate;

        _repositoryManager.CommentRepository.Create(newComment);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var deleteComment = await _repositoryManager.CommentRepository.GetByIdAsync(id, true, cancellationToken);
        if (deleteComment is null)
        {
            throw new CommentNotFoundException();
        }

        _repositoryManager.CommentRepository.Delete(deleteComment);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CommentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {

        var comments = await _repositoryManager.CommentRepository.GetAllAsync(cancellationToken: cancellationToken);
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

    public async Task UpdateAsync(Guid commentId, Guid modifierId, CommentUpdateRequest updatedComment, CancellationToken cancellationToken = default)
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
}
