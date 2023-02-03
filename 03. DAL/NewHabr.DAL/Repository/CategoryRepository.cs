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
        ArgumentNullException.ThrowIfNull(category, nameof(category));
        Delete(category);
    }

    public async Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException(name, nameof(name));
        }
        var category = await Set.FirstOrDefaultAsync(c => c.Name == name && !c.Deleted);
        ArgumentNullException.ThrowIfNull(category, nameof(category));
        Delete(category);
    }

    public new async Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await FindByCondition(c => !c.Deleted).ToListAsync(cancellationToken);
    }
}
