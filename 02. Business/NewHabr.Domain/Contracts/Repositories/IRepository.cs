using System.Linq.Expressions;

namespace NewHabr.Domain.Contracts;

public interface IRepository<TEntity, TKey>
{
    //IQueryable<TEntity> FindAll(bool trackChanges = false);
    //IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false);
    //IQueryable<TEntity> GetAll(bool trackChanges = false);
    //IQueryable<TEntity> GetDeleted(bool trackChanges = false);
    //IQueryable<TEntity> GetAvailable(bool trackChanges = false);
    //IQueryable<TEntity?> GetById(TKey id, bool trackChanges = false);
    void Create(TEntity data);
    void Update(TEntity data);
    void Delete(TEntity data);
}
