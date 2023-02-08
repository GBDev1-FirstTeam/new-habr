using System;
namespace NewHabr.Domain.Models;

public class UserComment
{
    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
    public string Text { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int LikesCount { get; set; }
}

