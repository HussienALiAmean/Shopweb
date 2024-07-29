using Shop_web_API.Repositories.GenericRepository;
using Shop_web_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Shop_web_API.Models.enums;
using System.Transactions;
using Shop_web_API.Repositories.ProductRepository;
using Shop_web_API.Models.DTOs;
using AutoMapper;

namespace Shop_web_API.Service.OrderService
{
    public class OrderService
    {
        private readonly IMapper _mapper;
        private IGenericRepository<Order> _orderRepository {  get; set; }
        private IProductRepository _productRepository {  get; set; }
      
        public OrderService(IMapper mapper,IGenericRepository<Order> orderRepository, IProductRepository productRepository)
        {
                _mapper = mapper;
                this._orderRepository = orderRepository;
                this._productRepository = productRepository;
        }

        ///  <summary>
        ///  Get all orders for the Admin Products 
        ///  </summary>
        ///  <param></param>
        ///  <returns> IEnumerable<Order> </returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAll().Include(o => o.OrderProducts).ThenInclude(op => op.Product).ToListAsync();
            var orderDTO = _mapper.Map<IEnumerable<OrderDTO>>(orders);
            return orderDTO;
        }

        ///  <summary>
        ///  Get all orders for a signed user 
        ///  </summary>
        ///  <param></param>
        ///  <returns> IEnumerable<Order> </returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        public async Task<IEnumerable<OrderDTO>> GetOrdersAsync(string userId)
        {
            var orders = await _orderRepository.GetAll().Where(o => o.UserId == userId).Include(o => o.OrderProducts).ThenInclude(op => op.Product).ToListAsync();
            var orderDTO = _mapper.Map<IEnumerable<OrderDTO>>(orders);
            return orderDTO;
        }

        ///  <summary>
        ///  Post all orders by a signed user 
        ///  </summary>
        ///  <param name="orderProducts"></param>
        ///  <returns> added order </returns>
        ///  Author:AL Hussien Ali (AHA)   Date: 26-07-2024
        ///   Notes:
        ///   Edit History:
        ///   Name:           Date:        Description:
        ///   Revision History:
        ///   Name:           Date:        Description:
        ///   
        ///----------------------------------------------------------------- 
        public async Task<OrderStatus> AddOrderAsync(List<OrderProduct> orderProducts, string userId)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                OrderProducts = orderProducts
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {

                foreach (var orderProduct in orderProducts)
                {
                    var product = await _productRepository.GetByIdAsync(orderProduct.ProductId);

                    if (product == null)
                    {
                        return OrderStatus.ProductQuantityOutOfStock;
                    }
                    else if (product.Quantity == 0)
                    {
                        return OrderStatus.ProductQuantityOutOfStock;
                    }
                    else if (!product.IsVisible)
                    {
                        return OrderStatus.ProductIsNotVisible;
                    }
                    else if (product.Quantity < orderProduct.Quantity)
                    {
                        return OrderStatus.ProductQuantityExceededCurruntQuantity;
                    }
                    else
                    {
                        product.Quantity -= orderProduct.Quantity;
                        await _productRepository.UpdateAsync(product);
                        orderProduct.Product = product;
                    }
                }
                await _orderRepository.AddAsync(order);
                scope.Complete();
                return OrderStatus.OrderDone;

            }
            catch (Exception ex)
            {
                return OrderStatus.OrderError;

            }
        }
    }
}
