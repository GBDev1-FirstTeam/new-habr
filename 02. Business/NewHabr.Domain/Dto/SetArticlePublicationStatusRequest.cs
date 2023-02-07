#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class SetArticlePublicationStatusRequest
{
    [Required]
    public Guid Id { get; set; }

    public bool PublicationStatus { get; set; }
}
