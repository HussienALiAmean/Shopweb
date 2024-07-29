using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop_web_API.Controllers;
using Shop_web_API.Models.DTOs;
using Shop_web_API.Service.OrderService;
using System.Security.Claims;

namespace ShopWebAPITest
{
    public class OrdersControllerTests
    {

        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<OrderService> _orderServiceMock;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _orderServiceMock = new Mock<OrderService>(Mock.Of<IMapper>());
            _controller = new OrdersController(_mapperMock.Object, _orderServiceMock.Object);
        }

        [Fact]
        public async Task GetAllOrders_AdminRole_ReturnsOk()
        {
            // Arrange
            _orderServiceMock.Setup(s => s.GetAllOrdersAsync()).ReturnsAsync(new List<OrderDTO> { new OrderDTO() });

            // Act
            var result = await _controller.GetAllOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotEmpty((IEnumerable<OrderDTO>)okResult.Value);
        }

        [Fact]
        public async Task GetOrders_UserRole_ReturnsOk()
        {
            // Arrange
            var userId = "61e528cd-37fc-4503-b1dc-9eda07f28935";
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                    new Claim("UserId", userId)
                    }))
                }
            };

            _orderServiceMock.Setup(s => s.GetOrdersAsync(userId)).ReturnsAsync(new List<OrderDTO> { new OrderDTO() });

            // Act
            var result = await _controller.GetOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotEmpty((IEnumerable<OrderDTO>)okResult.Value);
        }
    }
}
