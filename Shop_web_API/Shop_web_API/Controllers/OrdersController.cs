using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_web_API.Models;
using Shop_web_API.Models.DTOs;
using Shop_web_API.Models.enums;
using Shop_web_API.Service.OrderService;
using System.Security.Claims;

namespace Shop_web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly OrderService _orderService;

        public OrdersController(IMapper mapper,OrderService orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        ///  <summary>
        ///  Get all orders for the Admin Products 
        ///  </summary>
        ///  <param></param>
        ///  <returns>Http Response of get an IEnumerable<Order> </returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            if (orders == null || orders.Count() <= 0)
            {
                return NoContent();
            }
            return Ok(orders);
        }


        ///  <summary>
        ///  Get all orders for a signed user 
        ///  </summary>
        ///  <param></param>
        ///  <returns>Http response of get an IEnumerable<Order> </returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetOrders()
        {   //get the userId to get all the user Orders
            var userId = User.FindFirstValue("UserId");
            var orders = await _orderService.GetOrdersAsync(userId);
            if (orders == null || orders.Count() <=0)
            {
                return NoContent();
            }
            return Ok(orders);
        }

        ///  <summary>
        ///  Post all orders by a signed user 
        ///  </summary>
        ///  <param name="orderProducts"></param>
        ///  <returns>Http response of add new order  </returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderStatus>> PostOrder([FromBody] List<OrderProductDTO> orderProductsDTO)
        {
            //map opjects
            var orderProducts = _mapper.Map<List<OrderProduct>>(orderProductsDTO);

            // get the user id from Claims to make a post Order
            var userId = User.FindFirstValue("UserId");
            var orderStatus = await  _orderService.AddOrderAsync(orderProducts, userId);

            if (orderStatus != OrderStatus.OrderDone)
            {
                return BadRequest(orderStatus.ToString());
            }
            return Ok(orderStatus.ToString());
        }
    }
}
