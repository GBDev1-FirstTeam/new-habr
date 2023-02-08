using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewHabr.Domain.Dto;

public class UserForManipulationDto
{
    [MaxLength(30)]
    public string? FirstName { get; set; }

    [MaxLength(30)]
    public string? LastName { get; set; }

    [MaxLength(30)]
    public string? Patronymic { get; set; }

    public long? BirthDay { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }
}

public class UserProfileDto : UserForManipulationDto
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public int? Age { get; set; }

    public bool Banned { get; set; }

    public string? BanReason { get; set; }

    public long? BanExpiratonDate { get; set; }

    public long? BannedAt { get; set; }

    public int ReceivedLikes { get; set; }
}
