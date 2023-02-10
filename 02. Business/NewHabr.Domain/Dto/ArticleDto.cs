#nullable disable
using System.ComponentModel.DataAnnotations;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Dto;

public class ArticleDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [MinLength(3), MaxLength(500)]
    public string Title { get; set; }

    public ICollection<CategoryDto> Categories { get; set; } = new HashSet<CategoryDto>();

    public ICollection<TagDto> Tags { get; set; } = new HashSet<TagDto>();

    public ICollection<CommentDto> Comments { get; set; } = new HashSet<CommentDto>();

    public string Content { get; set; }

    public long CreatedAt { get; set; }

    public long ModifiedAt { get; set; }

    public long PublishedAt { get; set; }

    public long DeletedAt { get; set; }

    public bool Published { get; set; }

    public bool Deleted { get; set; }

    public ApproveState ApproveState { get; set; }

    public override bool Equals(object obj) => obj is ArticleDto ? Equals(obj as ArticleDto) : base.Equals(obj);
    public override int GetHashCode()
    {
        return
            Id.GetHashCode() +
            Title.GetHashCode() +
            UserId.GetHashCode() +
            Categories.GetHashCode() +
            Comments.GetHashCode() +
            Tags.GetHashCode() +
            Content.GetHashCode() +
            CreatedAt.GetHashCode() +
            PublishedAt.GetHashCode() +
            DeletedAt.GetHashCode();
    }
    public bool Equals(ArticleDto article)
    {
        return Id == article.Id
            && UserId == article.UserId
            && Title == article.Title
            && Content == article.Content
            && CreatedAt == article.CreatedAt
            && ModifiedAt == article.ModifiedAt
            && Published == article.Published
            && DeletedAt == article.DeletedAt
            && ApproveState == article.ApproveState
            && Published == article.Published
            && Deleted == article.Deleted;
    }
    public static bool operator ==(ArticleDto a1, ArticleDto a2) => a1.Equals(a2);
    public static bool operator !=(ArticleDto a1, ArticleDto a2) => !a1.Equals(a2);
}
