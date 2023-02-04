using System.Linq.Expressions;

namespace NewHabr.Domain.Contracts;

public interface IRepository<TEntity, TKey>
{
    IQueryable<TEntity> FindAll(bool trackChanges);
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false);
    void Create(TEntity data);
    void Update(TEntity data);
    void Delete(TEntity data);
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TEntity>> GetDeletedAsync(CancellationToken cancellationToken = default);
}
