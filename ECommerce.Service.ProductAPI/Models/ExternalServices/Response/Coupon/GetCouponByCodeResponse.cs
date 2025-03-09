namespace ECommerce.Service.ProductAPI.Models.ExternalServices.Response.Coupon
{
    public class GetCouponByCodeResponse
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
