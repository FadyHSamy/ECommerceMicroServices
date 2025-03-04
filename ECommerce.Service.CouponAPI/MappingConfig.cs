using AutoMapper;
using ECommerce.Service.CouponAPI.Dto;
using ECommerce.Service.CouponAPI.Models;

namespace ECommerce.Service.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Coupon, CouponDto>().ReverseMap();
            });
            return config;
        }
    }
}
