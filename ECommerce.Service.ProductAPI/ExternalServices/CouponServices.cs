using ECommerce.Service.ProductAPI.Dto;
using ECommerce.Service.ProductAPI.ExternalServices.Interface;
using ECommerce.Service.ProductAPI.Models.ExternalServices.Response.Coupon;
using Newtonsoft.Json;

namespace ECommerce.Service.ProductAPI.ExternalServices
{
    public class CouponServices : ICouponServices
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponServices(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }
        public async Task<GetCouponByCodeResponse?> GetCouponByCode(string couponCode)
        {
            var coupon = _httpClientFactory.CreateClient("Coupon");

            var couponResponse = await coupon.GetAsync($"api/CouponAPI/GetByCode/{couponCode}");

            var apiContent = await couponResponse.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (response != null && response.IsSuccess && response.Result != null)
            {
                var resultString = Convert.ToString(response.Result);
                if (!string.IsNullOrEmpty(resultString))
                {
                    var couponList = JsonConvert.DeserializeObject<IEnumerable<GetCouponByCodeResponse>>(resultString);
                    return couponList?.FirstOrDefault();
                }
            }

            return null;
        }
    }
}
