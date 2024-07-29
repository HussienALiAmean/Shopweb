using Microsoft.EntityFrameworkCore;
using Shop_web_API.Data;
using Shop_web_API.Models;
using Shop_web_API.Repositories.GenericRepository;

namespace Shop_web_API.Repositories.ProductRepository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetVisibleProductsAsync()
        {
            try
            {
                return await this.GetAll().Where(p => p.IsVisible).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
           
        }
    }
}