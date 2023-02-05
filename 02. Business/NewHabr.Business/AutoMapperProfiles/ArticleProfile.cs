using AutoMapper;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Business.AutoMapperProfiles;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<Article, ArticleDto>().ReverseMap();
        CreateMap<CreateArticleRequest, Article>();
        CreateMap<UpdateArticleRequest, Article>();
    }
}
