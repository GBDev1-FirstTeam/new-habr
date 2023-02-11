using AutoMapper;
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

    public async Task CreateAsync(Guid CreatorId, CreateCommentRequest data, CancellationToken cancellationToken = default)
    {
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
        var deleteComment = await _repositoryManager.CommentRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
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

    public async Task UpdateAsync(Guid commentId, Guid modifierId, UpdateCommentRequest updatedComment, CancellationToken cancellationToken = default)
    {
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
}
