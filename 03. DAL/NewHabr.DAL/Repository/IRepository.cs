namespace NewHabr.DAL.Repository;
public interface IRepository<T,Tkey>
{
    List<T> GetAll();
    T GetById(Tkey id);
    Tkey Create(T data);
    int Update(T data);
    int Delete(Tkey id);
}
