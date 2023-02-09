using AutoMapper;
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
            .ForMember(dest => dest.PublishedAt, e => e.MapFrom(src => src.PublishedAt.Value.ToUnixTimeMilliseconds()))
            .ForMember(dest => dest.DeletedAt, e => e.MapFrom(src => src.DeletedAt.Value.ToUnixTimeMilliseconds()));

        CreateMap<ArticleDto, Article>()
            .ForMember(dest => dest.CreatedAt, e => e.MapFrom(src => new DateTimeOffset(new DateTime(src.CreatedAt))))
            .ForMember(dest => dest.ModifiedAt, e => e.MapFrom(src => new DateTimeOffset(new DateTime(src.ModifiedAt))))
            .ForMember(dest => dest.PublishedAt, e => e.MapFrom(src => new DateTimeOffset(new DateTime(src.PublishedAt))))
            .ForMember(dest => dest.DeletedAt, e => e.MapFrom(src => new DateTimeOffset(new DateTime(src.DeletedAt))));

        CreateMap<CreateArticleRequest, Article>()
            .ForMember(dest => dest.Categories, options => options.Ignore())
            .ForMember(dest => dest.Tags, options => options.Ignore());

        CreateMap<UpdateArticleRequest, Article>()
            .ForMember(dest => dest.Categories, options => options.Ignore())
            .ForMember(dest => dest.Tags, options => options.Ignore());
    }
}
