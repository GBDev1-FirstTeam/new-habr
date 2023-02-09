#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Dto;

public class ArticleDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [NotNull, MinLength(3), MaxLength(500)]
    public string Title { get; set; }

    public CategoryDto[] Categories { get; set; } = Array.Empty<CategoryDto>();

    public TagDto[] Tags { get; set; } = Array.Empty<TagDto>();

    public CommentDto[] Comments { get; set; } = Array.Empty<CommentDto>();

    [NotNull]
    public string Content { get; set; }

    public long CreatedAt { get; set; }

    public long ModifiedAt { get; set; }

    public long PublishedAt { get; set; }

    public long DeletedAt { get; set; }

    public bool Published { get; set; }

    public bool Deleted { get; set; }

    public ApproveState ApproveState { get; set; }

    public override bool Equals(object obj) => obj is ArticleDto ? Equals(obj as ArticleDto) : base.Equals(obj);
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
