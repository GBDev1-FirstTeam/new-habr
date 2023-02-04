using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public abstract class ReporitoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : struct
{
    private readonly ApplicationContext _context;
    protected DbSet<TEntity> Set;

    public ReporitoryBase(ApplicationContext context)
    {
        _context = context;
        Set = _context.Set<TEntity>();
    }

    public void Create(TEntity data)
    {
        Set.Add(data);
    }

    public void Delete(TEntity data)
    {
        data.Deleted = true;
        Update(data);
    }

    public void Update(TEntity data)
    {
        Set.Update(data);
    }

    public IQueryable<TEntity> FindAll(bool trackChanges = false)
    {
        return trackChanges ? Set : Set.AsNoTracking();
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false)
    {
        return FindAll(trackChanges).Where(expression);
    }

    public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await FindAll().ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await Set.FirstOrDefaultAsync(e => e.Id.Equals(id) && !e.Deleted);
    }

    public virtual async Task<IReadOnlyCollection<TEntity>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await FindByCondition(a => a.Deleted).ToListAsync(cancellationToken);
    }
}
