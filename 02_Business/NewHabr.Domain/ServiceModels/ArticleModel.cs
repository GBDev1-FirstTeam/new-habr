#nullable disable

using System.ComponentModel.DataAnnotations;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.ServiceModels;

public class ArticleModel
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public Guid UserId { get; set; }

    public string UserName { get; set; }

    public string ImgURL { get; set; }

    public ICollection<CommentModel> Comments { get; set; }

    public ICollection<Category> Categories { get; set; }

    public ICollection<Tag> Tags { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }

    public bool Published { get; set; }

    public DateTimeOffset? PublishedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public ApproveState ApproveState { get; set; }

    public int LikesCount { get; set; }

    public int CommentsCount { get; set; }

    public bool IsLiked { get; set; }

    public bool Deleted { get; set; }
}
