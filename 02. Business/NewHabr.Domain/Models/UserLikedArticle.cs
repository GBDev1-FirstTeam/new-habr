using System;
using NewHabr.Domain.Dto;

namespace NewHabr.Domain.Models;

public class UserLikedArticle
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public ICollection<Category> Categories { get; set; }

    public ICollection<Tag> Tags { get; set; }
}

public class UserLikedComment
{
    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
    public string ArticleTitle { get; set; }
    public string Text { get; set; }
}

public class UserLikedUser
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
}