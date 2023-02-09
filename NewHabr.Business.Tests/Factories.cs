using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewHabr.Business.AutoMapperProfiles;
using NewHabr.DAL.EF;

namespace NewHabr.Business.Tests;

public static class Factories
{
    public static ApplicationContext GetDataContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
                     .UseInMemoryDatabase(Guid.NewGuid().ToString())
                     .Options;
        var db = new ApplicationContext(options);
        return db;
    }

    public static IMapper GetMapper()
    {
        var mapperConfigurations = new MapperConfiguration(config => config.AddMaps(typeof(ArticleProfile).Assembly));
        return mapperConfigurations.CreateMapper();
    }
}
