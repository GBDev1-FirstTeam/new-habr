using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity>
    where TEntity : class, IEntity<TKey>
    where TKey : struct
{
    private readonly ApplicationContext _context;
    protected DbSet<TEntity> Set;

    public RepositoryBase(ApplicationContext context)
    {
        _context = context;
        Set = _context.Set<TEntity>();
    }

    public void Create(TEntity data)
    {
        Set.Add(data);
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
        return await FindAll(trackChanges).ToListAsync(cancellationToken);
    }

    public IQueryable<TEntity> GetAvailable(bool trackChanges = false)
    {
        return FindByCondition(a => !a.Deleted, trackChanges);
    }
}
