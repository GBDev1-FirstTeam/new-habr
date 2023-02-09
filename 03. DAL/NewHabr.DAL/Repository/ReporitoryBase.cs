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

    public TKey Create(TEntity data)
    {
        return Set.Add(data).Entity.Id;
    }

    public virtual void Delete(TEntity data)
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

    public IQueryable<TEntity?> GetById(TKey id, bool trackChanges = false)
    {
        return FindByCondition(e => e.Id.Equals(id) && !e.Deleted, trackChanges);
    }

    public IQueryable<TEntity> GetAll(bool trackChanges = false)
    {
        return FindAll(trackChanges);
    }

    public IQueryable<TEntity> GetDeleted(bool trackChanges = false)
    {
        return FindByCondition(a => a.Deleted, trackChanges);
    }

    public IQueryable<TEntity> GetAvailable(bool trackChanges = false)
    {
        return FindByCondition(a => !a.Deleted, trackChanges);
    }
}
