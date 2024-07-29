using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop_web_API.Controllers;
using Shop_web_API.Models;
using Shop_web_API.Models.DTOs;
using Shop_web_API.Service.ProductService;

namespace ShopWebAPITest
{
    public class ProductsControllerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ProductService> _mockProductService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockProductService = new Mock<ProductService>();
            _controller = new ProductsController(_mockMapper.Object, _mockProductService.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
        {
            var products = new List<Product>
                {
                    new Product { Id = 1, Name = "Product1", Description = "Desc1", Price = 10.0m, Quantity = 5, IsVisible = true },
                    new Product { Id = 2, Name = "Product2", Description = "Desc2", Price = 20.0m, Quantity = 10, IsVisible = false }
                };

            _mockProductService.Setup(service => service.GetAllProductsAsync())
                               .ReturnsAsync(products);  // Corrected line

            var productDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Quantity = p.Quantity,
                IsVisible = p.IsVisible
            }).ToList();

            _mockMapper.Setup(m => m.Map<IEnumerable<ProductDTO>>(It.IsAny<IEnumerable<Product>>()))
                       .Returns(productDTOs);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProducts = Assert.IsType<List<ProductDTO>>(okResult.Value);
            Assert.Equal(2, returnProducts.Count);
        }

        [Fact]
        public async Task GetProducts_ReturnsNoContent_WhenNoProducts()
        {
            // Arrange
            var products = new List<Product>();
            _mockProductService.Setup(service => service.GetAllProductsAsync())
                               .ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task GetProduct_ReturnsOkResult_WithProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1", Description = "Desc1", Price = 10.0m, Quantity = 5, IsVisible = true };
            _mockProductService.Setup(service => service.GetProductAsync(1))
                               .ReturnsAsync(product);
            var productDTO = new ProductDTO { Id = 1, Name = "Product1", Description = "Desc1", Price = 10.0m, Quantity = 5, IsVisible = true };
            _mockMapper.Setup(m => m.Map<ProductDTO>(It.IsAny<Product>()))
                       .Returns(productDTO);

            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProduct = Assert.IsType<ProductDTO>(okResult.Value);
            Assert.Equal(product.Id, returnProduct.Id);
            Assert.Equal(product.Name, returnProduct.Name);
            Assert.Equal(product.Description, returnProduct.Description);
            Assert.Equal(product.Price, returnProduct.Price);
            Assert.Equal(product.Quantity, returnProduct.Quantity);
            Assert.Equal(product.IsVisible, returnProduct.IsVisible);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenProductNotFound()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProductAsync(1))
                               .ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostProduct_ReturnsOkResult_WithCreatedProduct()
        {
            // Arrange
            var productDTO = new ProductDTO { Id = 1, Name = "Product1", Description = "Desc1", Price = 10.0m, Quantity = 5, IsVisible = true };
            var product = new Product { Id = 1, Name = "Product1", Description = "Desc1", Price = 10.0m, Quantity = 5, IsVisible = true };
            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<ProductDTO>()))
                       .Returns(product);
            _mockProductService.Setup(service => service.AddProductAsync(product))
                               .ReturnsAsync(product);

            // Act
            var result = await _controller.PostProduct(productDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(product.Id, returnProduct.Id);
            Assert.Equal(product.Name, returnProduct.Name);
            Assert.Equal(product.Description, returnProduct.Description);
            Assert.Equal(product.Price, returnProduct.Price);
            Assert.Equal(product.Quantity, returnProduct.Quantity);
            Assert.Equal(product.IsVisible, returnProduct.IsVisible);
        }

        [Fact]
        public async Task PostProduct_ReturnsBadRequest_WhenProductIsNull()
        {
            // Arrange
            var productDTO = new ProductDTO { Id = 1, Name = "Product1", Description = "Desc1", Price = 10.0m, Quantity = 5, IsVisible = true };
            var product = new Product { Id = 1, Name = "Product1", Description = "Desc1", Price = 10.0m, Quantity = 5, IsVisible = true };
            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<ProductDTO>()))
                       .Returns(product);
            _mockProductService.Setup(service => service.AddProductAsync(product))
                               .ReturnsAsync((Product)null);

            // Act
            var result = await _controller.PostProduct(productDTO);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task PutProduct_ReturnsOkResult_WithUpdatedProduct()
        {
            // Arrange
            var productDTO = new ProductDTO { Id = 1, Name = "Product1", Description = "Desc1", Price = 10.0m, Quantity = 5, IsVisible = true };
            var product = new Product { Id = 1, Name = "Product1", Description = "Desc1", Price = 10.0m, Quantity = 5, IsVisible = true };
            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<ProductDTO>()))
                       .Returns(product);
            _mockProductService.Setup(service => service.PutProductAsync(1, product))
                               .ReturnsAsync(product);

            // Act
            var result = await _controller.PutProduct(1, productDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(product.Id, returnProduct.Id);
            Assert.Equal(product.Name, returnProduct.Name);
            Assert.Equal(product.Description, returnProduct.Description);
            Assert.Equal(product.Price, returnProduct.Price);
            Assert.Equal(product.Quantity, returnProduct.Quantity);
            Assert.Equal(product.IsVisible, returnProduct.IsVisible);
        }

        [Fact]
        public async Task PutProduct_ReturnsNotFound_WhenProductNotFound()
        {
            // Arrange
            var productDTO = new ProductDTO { Id = 1, Name = "Product1", Description = "Desc1", Price = 10.0m, Quantity = 5, IsVisible = true };
            var product = new Product { Id = 1, Name = "Product1", Description = "Desc1", Price = 10.0m, Quantity = 5, IsVisible = true };
            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<ProductDTO>()))
                       .Returns(product);
            _mockProductService.Setup(service => service.PutProductAsync(1, product))
                               .ReturnsAsync((Product)null);
            _mockProductService.Setup(service => service.GetProductAsync(1))
                               .ReturnsAsync((Product)null);

            // Act
            var result = await _controller.PutProduct(1, productDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsOkResult_WhenProductDeleted()
        {
            // Arrange
            _mockProductService.Setup(service => service.DeleteProduct(1))
                               .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNotFound_WhenProductNotFound()
        {
            // Arrange
            _mockProductService.Setup(service => service.DeleteProduct(1))
                               .ReturnsAsync(false);
            _mockProductService.Setup(service => service.GetProductAsync(1))
                               .ReturnsAsync((Product)null);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}