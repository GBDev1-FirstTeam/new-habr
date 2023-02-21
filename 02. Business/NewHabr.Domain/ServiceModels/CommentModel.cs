#nullable disable

using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Models;

public class CommentModel
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string UserName { get; set; }

    public Guid ArticleId { get; set; }

    public string Text { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }

    public int LikesCount { get; set; }

    public bool IsLiked { get; set; }

    public bool Deleted { get; set; }
}
