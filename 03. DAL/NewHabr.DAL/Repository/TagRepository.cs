using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public class TagRepository : ReporitoryBase<Tag, int>, ITagRepository
{
    public TagRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var tag = await Set.FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);
        ArgumentNullException.ThrowIfNull(tag, nameof(tag));
        Delete(tag);
    }

    public async Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException(name, nameof(name));
        }
        var tag = await Set.FirstOrDefaultAsync(c => c.Name == name && !c.Deleted);
        ArgumentNullException.ThrowIfNull(tag, nameof(tag));
        Delete(tag);
    }

    public new async Task<IReadOnlyCollection<Tag>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await FindByCondition(t => !t.Deleted).ToListAsync(cancellationToken);
    }
}
