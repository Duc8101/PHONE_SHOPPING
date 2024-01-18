﻿using API.Services;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService service;
        public OrderController(OrderService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<ResponseDTO<List<CartListDTO>?>> Create([Required] OrderCreateDTO DTO)
        {
            ResponseDTO<List<CartListDTO>?> response = await service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet]
        public async Task<ResponseDTO<PagedResultDTO<OrderListDTO>?>> List(Guid? UserID, string? status, [Required] int page = 1)
        {
            ResponseDTO<PagedResultDTO<OrderListDTO>?> response = await service.List(UserID, status, page);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
