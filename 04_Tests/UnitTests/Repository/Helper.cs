using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Models;

namespace UnitTests.Repository;

internal class Helper
{
    public static ApplicationContext InitContext()
    {
        var builder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        return new ApplicationContext(builder.Options);
    }

    public static User CreateUser()
    {
        return new User
        {
            UserName = $"JohnDoe_{Guid.NewGuid()}",
            SecureQuestion = new SecureQuestion { Question = Guid.NewGuid().ToString() },
            SecureAnswer = Guid.NewGuid().ToString()
        };
    }

    public static Article CreateArticle(User user = null)
    {
        return new Article()
        {
            Title = $"title_{Guid.NewGuid()}",
            Content = $"content_{Guid.NewGuid()}",
            CreatedAt = DateTimeOffset.Now,
            ModifiedAt = DateTimeOffset.Now,
            User = user ?? Helper.CreateUser()
        };
    }
}

