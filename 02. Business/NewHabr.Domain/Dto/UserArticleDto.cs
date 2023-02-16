#nullable disable


namespace NewHabr.Domain.Dto;

public class UserArticleDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string ImgURL { get; set; }

    public ICollection<CategoryDto> Categories { get; set; }

    public ICollection<TagDto> Tags { get; set; }

    public int CommentsCount { get; set; }

    public int LikesCount { get; set; }

    public long CreatedAt { get; set; }

    public long ModifiedAt { get; set; }

    public long PublishedAt { get; set; }

}

