﻿using AutoMapper;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Business.AutoMapperProfiles;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<Article, ArticleDto>()
            .ForMember(dest => dest.CreatedAt, e => e.MapFrom(src => src.CreatedAt.ToUnixTimeMilliseconds()))
            .ForMember(dest => dest.ModifiedAt, e => e.MapFrom(src => src.ModifiedAt.ToUnixTimeMilliseconds()))
            .ForMember(dest => dest.PublishedAt, e => e.MapFrom(src => src.PublishedAt.Value.ToUnixTimeMilliseconds()));

        CreateMap<ArticleDto, Article>()
            .ForMember(dest => dest.CreatedAt, e => e.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(src.CreatedAt)))
            .ForMember(dest => dest.ModifiedAt, e => e.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(src.ModifiedAt)))
            .ForMember(dest => dest.PublishedAt, e => e.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(src.PublishedAt)));

        CreateMap<ArticleCreateRequest, Article>()
            .ForMember(dest => dest.Categories, options => options.Ignore())
            .ForMember(dest => dest.Tags, options => options.Ignore());

        CreateMap<ArticleUpdateRequest, Article>()
            .ForMember(dest => dest.Categories, options => options.Ignore())
            .ForMember(dest => dest.Tags, options => options.Ignore());

        CreateMap<ArticleExt, ArticleDto>();
    }
}
