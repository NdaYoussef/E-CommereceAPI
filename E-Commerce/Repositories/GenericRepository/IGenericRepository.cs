namespace TestToken.Repositories.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
       public Task<T> GetByIdAsync(int id);
      // public Task<T> GetByName(string name);
       public Task<IEnumerable<T>> GetAllAsync();
       public Task<T> AddAsync (T entity);
       public Task<T> updateAsync(T entity);
       public Task<T> deleteAsync(int id);
    }
}
