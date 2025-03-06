using AutoMapper;
using Azure;
using ECommerce.Service.AuthAPI.Dto;
using ECommerce.Service.ProductAPI.Data;
using ECommerce.Service.ProductAPI.Dto;
using ECommerce.Service.ProductAPI.Models;
using ECommerce.Service.ProductAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Service.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly AppDbContext _context;
        public ProductController(IProductService productService, IMapper mapper, AppDbContext context)
        {
            _productService = productService;
            _response = new ResponseDto();
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> products = _context.Products.ToList();

                var couponsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

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
        [Route("{ProdcutId:int}")]
        public ResponseDto Get(int ProdcutId)
        {
            try
            {
                Product product = _context.Products.First((prodcut) => prodcut.ProductId == ProdcutId);

                var productDto = _mapper.Map<ProductDTO>(product);

                _response.Result = productDto;
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
        public ResponseDto Post([FromBody] ProductDTO request)
        {
            try
            {
                var product = _mapper.Map<Product>(request);

                _context.Products.Add(product);
                _context.SaveChanges();

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
        public ResponseDto Put([FromBody] ProductDTO request)
        {
            try
            {
                var product = _mapper.Map<Product>(request);

                _context.Products.Update(product);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                var product = _context.Products.First((product) => product.ProductId == id);
                _context.Products.Remove(product);
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
