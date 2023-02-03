namespace WeDoRolodex3000.Data;

public interface IBaseRepository<T> where T : class
{
    List<T> GetAll();
    void DeleteAll();
    Task<T> GetById(int id);
    Task Add(T entity);
    void Edit(T entity);
    Task DeleteById(int id);
    Task<int> SaveChanges();
    void RefreshData(T entity);

}
