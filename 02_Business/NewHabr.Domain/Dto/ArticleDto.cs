#nullable disable
using System.ComponentModel.DataAnnotations;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Dto;

public class ArticleDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Username { get; set; }

    public string Title { get; set; }

    public ICollection<CategoryDto> Categories { get; set; }

    public ICollection<TagDto> Tags { get; set; }

    public ICollection<CommentDto> Comments { get; set; }

    public string Content { get; set; }

    public long CreatedAt { get; set; }

    public long ModifiedAt { get; set; }

    public long PublishedAt { get; set; }

    public bool Published { get; set; }

    public ApproveState ApproveState { get; set; }

    public string ImgURL { get; set; }

    public bool IsLiked { get; set; }

    public int LikesCount { get; set; }

    public int CommentsCount { get; set; }
}
