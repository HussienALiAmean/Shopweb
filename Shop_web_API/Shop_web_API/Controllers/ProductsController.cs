using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_web_API.Models;
using Shop_web_API.Models.DTOs;
using Shop_web_API.Service.ProductService;

namespace Shop_web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ProductService _productService;

        public ProductsController(IMapper mapper,ProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        ///  <summary>
        ///  Get all products for the Admin  
        ///  </summary>
        ///  <param></param>
        ///  <returns>Http Response of get an IEnumerable<Products> </returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {

            var products = await _productService.GetAllProductsAsync();
            if (products != null || products.Count() > 0)
            {
                return Ok(products);
            }
            else
            {
                return NoContent();
            }
        }

        ///  <summary>
        ///  Get all visible products for the users  
        ///  </summary>
        ///  <param></param>
        ///  <returns>Http Response of get an IEnumerable<Products> </returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        [HttpGet("visible")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetVisibleProducts()
        {
            var products =  await _productService.GetVisibleProductsAsync();
            if (products != null || products.Count() > 0)
            {
                return Ok(products);
            }
            else
            {
                return NoContent();
            }
        }

        ///  <summary>
        ///  Get a product   
        ///  </summary>
        ///  <param name="id"></param>
        ///  <returns>Http response of get a Product</returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _productService.GetProductAsync(id);

            if (product == null )
            {
                return NotFound();
            }
            return Ok(product);
        }

        ///  <summary>
        ///  Post a product   
        ///  </summary>
        ///  <param name="product"></param>
        ///  <returns>Http response of a Post new Products</returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] ProductDTO newProductDTO)
        {
            var newProduct = _mapper.Map<Product>(newProductDTO);
            var product = await _productService.AddProductAsync(newProduct);
            if (product == null)
            {
                return BadRequest();
            }
            return Ok(product);
        }

        ///  <summary>
        ///  Put a product   
        ///  </summary>
        ///  <param name="product"></param>
        ///  <param name="id"></param>
        ///  <returns>Http response of an update Product</returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            var updatedProduct =  await _productService.PutProductAsync(id, product);
            if(updatedProduct == null)
            {
                if (await _productService.GetProductAsync(id) != null)
                {
                    return BadRequest();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return Ok(updatedProduct);
            }
        }


        ///  <summary>
        ///  delete a product   
        ///  </summary>
        ///  <param name="id"></param>
        ///  <returns>Http response of delete a Products</returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var operationResult = await _productService.DeleteProduct(id);
            if (operationResult == false)
            {
                if (await _productService.GetProductAsync(id) != null)
                {
                    return BadRequest();
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok(operationResult);
        }
       
    }
}