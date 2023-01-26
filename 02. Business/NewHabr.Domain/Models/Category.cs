namespace NewHabr.Domain.Models;

public class Category : BaseEntity<int>
{
    public string Text { get; set; } = string.Empty;
    public List<Article> Articles { get; set; } = new();
}
