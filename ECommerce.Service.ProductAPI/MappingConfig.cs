using AutoMapper;
using ECommerce.Service.ProductAPI.Dto;
using ECommerce.Service.ProductAPI.Models;

namespace ECommerce.Service.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>().ReverseMap();
            });
            return config;
        }
    }
}
