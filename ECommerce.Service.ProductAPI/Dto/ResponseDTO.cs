﻿namespace ECommerce.Service.ProductAPI.Dto
{
    public class ResponseDto
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; } = true;
        public object? Result { get; set; }
    }
}
