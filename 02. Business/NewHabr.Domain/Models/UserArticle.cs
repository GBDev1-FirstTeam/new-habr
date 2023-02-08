#nullable disable

namespace NewHabr.Domain.Models;

public class UserArticle
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public ICollection<Category> Categories { get; set; }

    public ICollection<Tag> Tags { get; set; }

    public int CommentsCount { get; set; }

    public int LikesCount { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }

    public DateTimeOffset PublishedAt { get; set; }
}
