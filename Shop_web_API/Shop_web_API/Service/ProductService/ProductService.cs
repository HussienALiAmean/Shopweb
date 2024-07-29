using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_web_API.Models;
using Shop_web_API.Models.DTOs;
using Shop_web_API.Repositories.ProductRepository;

namespace Shop_web_API.Service.ProductService
{
    public class ProductService
    {
        private readonly IMapper _mapper;
        private IProductRepository  _productRepository { get; set; }
        public ProductService(IMapper mapper,IProductRepository productRepository)
        {
            this._mapper = mapper;
            this._productRepository = productRepository;
        }


        ///  <summary>
        ///  Get all orders for the Admin Products 
        ///  </summary>
        ///  <param></param>
        ///  <returns>IEnumerable<Order> </returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetAll().ToListAsync(); ;
                var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
                return productsDTO;
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        ///  <summary>
        ///  Get all visible products for the users  
        ///  </summary>
        ///  <param></param>
        ///  <returns>IEnumerable<Products> </returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        public async Task<IEnumerable<ProductDTO>> GetVisibleProductsAsync()
        {
            var products = await _productRepository.GetVisibleProductsAsync();
            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return productsDTO;
        }

        ///  <summary>
        ///  Get a product   
        ///  </summary>
        ///  <param name="id"></param>
        ///  <returns>Product</returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        public async Task<ProductDTO> GetProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            var productDTO = _mapper.Map<ProductDTO>(product);
            return productDTO;
        }

        ///  <summary>
        ///  Post a product   
        ///  </summary>
        ///  <param name="product"></param>
        ///  <returns>Product</returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        public async Task<ProductDTO> AddProductAsync(Product newproduct)
        {
            var product = await _productRepository.AddAsync(newproduct);
            var productDTO = _mapper.Map<ProductDTO>(product);
            return productDTO;
        }

        ///  <summary>
        ///  Put a product   
        ///  </summary>
        ///  <param name="product"></param>
        ///  <param name="id"></param>
        ///  <returns>updated Product</returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        public async Task<ProductDTO> PutProductAsync(int id, Product product)
        {
            if (id != product.Id)
            {
                return null;
            }
            var updatedProduct = await _productRepository.UpdateAsync(product);
            var UpdatedProductDTO = _mapper.Map<ProductDTO>(updatedProduct);
            return UpdatedProductDTO;
        }

        ///  <summary>
        ///  delete a product   
        ///  </summary>
        ///  <param name="id"></param>
        ///  <returns>bool</returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        public async Task<bool> DeleteProduct(int id)
        {
            bool operationResult = await _productRepository.DeleteAsync(id);
            return operationResult;
        }
    }
}
