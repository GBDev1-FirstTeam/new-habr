#nullable disable

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
