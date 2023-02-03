namespace WeDoRolodex3000.Data;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    public RolodexContext db;
    public async Task Add(T entity)
    {
        await db.Set<T>().AddAsync(entity);
    }

    public async Task DeleteById(int id)
    {
        var q = await GetById(id);
        db.Set<T>().Remove(q);
    }

    public virtual void Edit(T entity)
    {
        db.Entry<T>(entity).State = EntityState.Modified;
    }

    public virtual List<T> GetAll()
    {
        return db
            .Set<T>().Select(a => a)
            .ToList();
    }

    public void DeleteAll()
    {
        db.Set<T>().RemoveRange(GetAll());
    }

    public virtual async Task<T> GetById(int id)
    {
        return await db.Set<T>().FindAsync(id);
    }

    public async Task<int> SaveChanges()
    {
        return await db.SaveChangesAsync();
    }

    public void RefreshData(T entity)
    {
        db.Entry<T>(entity).Reload();
    }
}
