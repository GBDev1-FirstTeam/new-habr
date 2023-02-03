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

    public override async Task<IReadOnlyCollection<Tag>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await FindByCondition(t => !t.Deleted).ToListAsync(cancellationToken);
    }
}
