using ECommerce.Service.ProductAPI.Models.ExternalServices.Response.Coupon;

namespace ECommerce.Service.ProductAPI.ExternalServices.Interface
{
    public interface ICouponServices
    {
        Task<GetCouponByCodeResponse> GetCouponByCode(string couponCode);
    }
}
