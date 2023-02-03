using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public class CategoryRepository : ReporitoryBase<Category, int>, ICategoryRepository
{
    public CategoryRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await Set.FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

        if (category is null)
        {
            throw new Exception("Category is not found.");
        }

        Delete(category);
    }

    public new async Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await FindByCondition(c => !c.Deleted).ToListAsync(cancellationToken);
    }
}
