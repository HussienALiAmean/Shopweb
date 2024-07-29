
namespace Shop_web_API.Repositories.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
