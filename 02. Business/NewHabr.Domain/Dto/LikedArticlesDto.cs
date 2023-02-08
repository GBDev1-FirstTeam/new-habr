using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class LikedArticleDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public ICollection<CategoryDto> Categories { get; set; }

    public ICollection<TagDto> Tags { get; set; }
}

public class LikedCommentDto
{
    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
    public string ArticleTitle { get; set; }
    public string Text { get; set; }
}

public class LikedUserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
}