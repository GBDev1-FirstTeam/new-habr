using AutoMapper;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Contracts.Services;
using NewHabr.Domain.Dto;
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

    public async Task CreateAsync(CreateCommentRequest data, CancellationToken cancellationToken = default)
    {
        var newComment = _mapper.Map<Comment>(data);
        _repositoryManager.CommentRepository.Create(newComment);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteAsync(CommentDto data, CancellationToken cancellationToken = default)
    {
        var deleteComment = _mapper.Map<Comment>(data);
        _repositoryManager.CommentRepository.Delete(deleteComment);
        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CommentDto>> GetAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var comments = await _repositoryManager.CommentRepository.GetAllAsync(trackChanges, cancellationToken);
        return _mapper.Map<List<CommentDto>>(comments);
    }

    public async Task<IReadOnlyCollection<CommentDto>> GetByArticleIdAsync(Guid articleId, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var comments = await _repositoryManager.CommentRepository.GetByArticleIdAsync(articleId, trackChanges, cancellationToken);
        return _mapper.Map<List<CommentDto>>(comments);
    }    

    public async Task<IReadOnlyCollection<CommentDto>> GetByUserIdAsync(Guid userId, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var comments = await _repositoryManager.CommentRepository.GetByUserIdAsync(userId, trackChanges, cancellationToken);
        return _mapper.Map<List<CommentDto>>(comments);
    }    

    public async Task UpdateAsync(CommentDto data, CancellationToken cancellationToken = default)
    {
        var updateComment = _mapper.Map<Comment>(data);
        _repositoryManager.CommentRepository.Update(updateComment);
        await _repositoryManager.SaveAsync(cancellationToken);
    }
}
