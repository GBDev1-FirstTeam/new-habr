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

        if (tag is null)
        {
            throw new Exception("Category is not found.");
        }

        Delete(tag);
    }

    public new async Task<IReadOnlyCollection<Tag>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await FindByCondition(t => !t.Deleted).ToListAsync(cancellationToken);
    }
}
