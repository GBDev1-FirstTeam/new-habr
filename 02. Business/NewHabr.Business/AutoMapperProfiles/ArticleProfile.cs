using AutoMapper;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;
using NewHabr.Domain.ServiceModels;

namespace NewHabr.Business.AutoMapperProfiles;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<Article, ArticleDto>()
            .ReverseMap();

        CreateMap<ArticleCreateRequest, Article>()
            .ForMember(dest => dest.Categories, options => options.Ignore())
            .ForMember(dest => dest.Tags, options => options.Ignore());

        CreateMap<ArticleUpdateRequest, Article>()
            .ForMember(dest => dest.Categories, options => options.Ignore())
            .ForMember(dest => dest.Tags, options => options.Ignore());

        CreateMap<ArticleModel, ArticleDto>();

        CreateMap<ArticleQueryParametersDto, ArticleQueryParameters>();
    }
}
