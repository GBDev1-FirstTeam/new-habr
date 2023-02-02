using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public class CategoryRepository : ReporitoryBase<Category, int>, ICategoryRepository
{
    public CategoryRepository(ApplicationContext context) : base(context)
    {
    }
}
