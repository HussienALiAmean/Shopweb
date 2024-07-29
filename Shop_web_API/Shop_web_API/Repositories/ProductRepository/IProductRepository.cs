using Shop_web_API.Models;
using Shop_web_API.Repositories.GenericRepository;

namespace Shop_web_API.Repositories.ProductRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetVisibleProductsAsync();
    }

}
