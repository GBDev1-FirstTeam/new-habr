#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class CommentDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Username { get; set; }

    public Guid ArticleId { get; set; }

    public string Text { get; set; }

    public long CreatedAt { get; set; }

    public long ModifiedAt { get; set; }

    public int LikesCount { get; set; }

    public bool IsLiked { get; set; }
}
