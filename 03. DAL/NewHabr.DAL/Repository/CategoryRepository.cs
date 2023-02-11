using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public class CategoryRepository : RepositoryBase<Category, int>, ICategoryRepository
{
    public CategoryRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<Category?> GetByIdAsync(int id, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        return await GetById(id, trackChanges).FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<Category?> GetByIdIncludeAsync(int id, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        return await GetById(id, trackChanges)
            .Include(c => c.Articles)
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<IReadOnlyCollection<Category>> GetAvaliableAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        return await GetAvailable(trackChanges).ToListAsync(cancellationToken);
    }
    public Task<Category?> GetByNameAsync(string name, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return FindByCondition(c => c.Name == name && !c.Deleted, trackChanges).FirstOrDefaultAsync(cancellationToken);
    }
}
