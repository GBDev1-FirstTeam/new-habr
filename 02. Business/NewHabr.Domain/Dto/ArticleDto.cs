﻿#nullable disable
using System.ComponentModel.DataAnnotations;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Dto;

public class ArticleDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required, MinLength(3), MaxLength(500)]
    public string Title { get; set; }

    public CategoryDto[] Categories { get; set; } = Array.Empty<CategoryDto>();

    public TagDto[] Tags { get; set; } = Array.Empty<TagDto>();

    public CommentDto[] Comments { get; set; } = Array.Empty<CommentDto>();

    [Required]
    public string Content { get; set; }

    public long CreatedAt { get; set; }

    public long ModifiedAt { get; set; }

    public long PublishedAt { get; set; }

    public bool Published { get; set; }

    public ApproveState ApproveState { get; set; }

    public string ImgURL { get; set; }
}
