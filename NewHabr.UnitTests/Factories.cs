using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;

namespace NewHabr.UnitTests;
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
}
