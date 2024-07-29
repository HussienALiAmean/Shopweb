using Shop_web_API.Models.DTOs;
using Shop_web_API.Models;
using AutoMapper;

namespace Shop_web_API.Profiles
{
   
        public class ProductProfile : Profile
        {
            public ProductProfile()
            {
                CreateMap<Product, ProductDTO>().ReverseMap();
            }
        }
    }
