using System.Diagnostics.CodeAnalysis;

namespace NewHabr.Domain.Dto;
public class UpdateCommentRequest
{
    [NotNull]
    public string Text { get; set; }
}
