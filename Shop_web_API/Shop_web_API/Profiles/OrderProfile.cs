using AutoMapper;
using Shop_web_API.Models.DTOs;
using Shop_web_API.Models;

namespace Shop_web_API.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>()
                .ReverseMap();
            CreateMap<OrderProduct, OrderProductDTO>()
                .ReverseMap();
        }
    }
}
