using System;
namespace NewHabr.Domain.Dto;

public class UserCommentDto
{
    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
    public string Text { get; set; }
    public long CreatedAt { get; set; }
    public int LikesCount { get; set; }
}

