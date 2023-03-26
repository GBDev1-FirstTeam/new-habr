using System;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;

namespace UnitTests.Repository;

public abstract class RepositoryTestsBase
{
    protected readonly ApplicationContext _context;
    protected readonly IFixture _fixture;


    public RepositoryTestsBase()
    {
        var builder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging();

        _context = new ApplicationContext(builder.Options);

        _fixture = new Fixture();
    }
}

