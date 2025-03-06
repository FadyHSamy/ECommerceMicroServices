using AutoMapper;
using ECommerce.Service.CouponAPI.Data;
using ECommerce.Service.CouponAPI.Dto;
using ECommerce.Service.CouponAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Service.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _context;
        private ResponseDto _response;
        private IMapper _mapper;

        public CouponAPIController(AppDbContext appDbContext, IMapper mapper)
        {
            _context = appDbContext;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> coupons = _context.Coupons.ToList();

                var couponsDto = _mapper.Map<IEnumerable<CouponDto>>(coupons);

                _response.Result = couponsDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{CouponId:int}")]
        public ResponseDto Get(int CouponId)
        {
            try
            {
                Coupon coupon = _context.Coupons.First((coupon) => coupon.CouponId == CouponId);

                var couponDto = _mapper.Map<CouponDto>(coupon);

                _response.Result = couponDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{Code}")]
        public ResponseDto GetByCode(string Code)
        {
            try
            {
                Coupon coupon = _context.Coupons.FirstOrDefault((coupon) => coupon.CouponCode.ToLower() == Code.ToLower());
                if (coupon == null)
                {
                    _response.IsSuccess = false;
                    return _response;
                }
                var couponDto = _mapper.Map<CouponDto>(coupon);

                _response.Result = couponDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);

                _context.Coupons.Add(coupon);
                _context.SaveChanges();

                couponDto = _mapper.Map<CouponDto>(coupon);

                _response.Result = couponDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);

                _context.Coupons.Update(coupon);
                _context.SaveChanges();

                couponDto = _mapper.Map<CouponDto>(coupon);

                _response.Result = couponDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        [Route("{id:int}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                var coupon = _context.Coupons.First((coupon) => coupon.CouponId == id);
                _context.Coupons.Remove(coupon);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
