using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public class TagRepository : ReporitoryBase<Tag, int>, ITagRepository
{
    public TagRepository(ApplicationContext context) : base(context)
    {
    }
}
