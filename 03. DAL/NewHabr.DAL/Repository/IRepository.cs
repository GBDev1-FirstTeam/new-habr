namespace NewHabr.DAL.Repository;
public interface IRepository<T,Tkey>
{
    Task<IReadOnlyCollection<T>> GetAll();
    Task<T> GetById(Tkey id);
    Task<Tkey> Create(T data);
    Task<int> Update(T data);
    Task<int> Delete(Tkey id);
}
