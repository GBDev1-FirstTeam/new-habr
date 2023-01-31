﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public abstract class ReporitoryBase<TEntity, TKey> : IRepository<TEntity>
    where TEntity : class, IEntity<TKey>
    where TKey : struct
{

    private readonly ApplicationContext _context;

    public ReporitoryBase(ApplicationContext context)
    {
        _context = context;
    }
    public void Create(TEntity data)
    {
       _context.Set<TEntity>().Add(data);        
    }

    public void Delete(TEntity data)
    {
        data.Deleted = true;
        Update(data);
    }

    public IQueryable<TEntity> FindAll(bool trackChanges = false)
    {
        return trackChanges ? _context.Set<TEntity>() : _context.Set<TEntity>().AsNoTracking();
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false)
    {
        return trackChanges ? _context.Set<TEntity>().Where(expression) : _context.Set<TEntity>().Where(expression).AsNoTracking();
    }

    public async Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await FindAll().ToListAsync();
    }

    public void Update(TEntity data)
    {
        _context.Set<TEntity>().Update(data);
    }
}
