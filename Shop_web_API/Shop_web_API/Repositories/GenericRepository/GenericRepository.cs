using Microsoft.EntityFrameworkCore;
using Shop_web_API.Data;

namespace Shop_web_API.Repositories.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public  IQueryable<T> GetAll()
        {
            try
            {
                return _dbSet.AsQueryable<T>();
            }
            catch (Exception)
            {
                return null;
            } 
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                try
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }
        }
    }
}
