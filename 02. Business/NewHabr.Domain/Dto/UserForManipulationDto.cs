using System;
using NewHabr.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public class UserForManipulationDto
{
    [MaxLength(30)]
    public string FirstName { get; set; }

    [MaxLength(30)]
    public string LastName { get; set; }

    [MaxLength(30)]
    public string Patronymic { get; set; }

    public DateTime? BirthDay { get; set; }

    [MaxLength(200)]
    public string Description { get; set; }
}

