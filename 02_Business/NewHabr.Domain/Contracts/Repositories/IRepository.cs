using System.Linq.Expressions;

namespace NewHabr.Domain.Contracts;

public interface IRepository<TEntity, TKey>
{
    void Create(TEntity data);
    void Update(TEntity data);
    void Delete(TEntity data);
}
