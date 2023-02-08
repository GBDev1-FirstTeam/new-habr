using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewHabr.Business.Services;
using NewHabr.DAL.EF;
using NewHabr.DAL.Repository;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace NewHabr.UnitTests;

public class ArticleTests
{
    private IRepositoryManager _repositoryManager;
    private IMapper _mapper;
    private ApplicationContext _context;
    
    [Fact]
    public async Task Test1()
    {
        _context = Factories.GetDataContext();
        _repositoryManager = new RepositoryManager(_context);
        var mapperConfigurations = new MapperConfiguration(config => config.AddMaps(typeof(Article).Assembly));
        _mapper = mapperConfigurations.CreateMapper();

        var articleService = new ArticleService(_repositoryManager, _mapper);

        var guid = Guid.NewGuid();
        _context.Articles.Add(new Article { Id = guid, Title = "" });
        _context.SaveChanges();

        await Assert.ThrowsAsync<ArticleNotFoundException>(async () => await articleService.GetByIdAsync(guid));
    }
}
