﻿using System.Linq.Expressions;

namespace NewHabr.Domain.Contracts;

public interface IRepository<TEntity>
{
    IQueryable<TEntity> FindAll(bool trackChanges);
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false);
    void Create(TEntity data);
    void Update(TEntity data);
    void Delete(TEntity data);
    Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}
