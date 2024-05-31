using API.Services.IService;
using DataAccess.DTO;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO.OrderDetailDTO;
using DataAccess.DTO.OrderDTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ResponseDTO<List<CartListDTO>?>> Create([Required] OrderCreateDTO DTO)
        {
            ResponseDTO<List<CartListDTO>?> response = await _service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet]
        public async Task<ResponseDTO<PagedResultDTO<OrderListDTO>?>> List(Guid? UserID, string? status, [Required] bool isAdmin = false, [Required] int page = 1)
        {
            ResponseDTO<PagedResultDTO<OrderListDTO>?> response = await _service.List(UserID, status, isAdmin, page);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet("{OrderID}")]
        public async Task<ResponseDTO<OrderDetailDTO?>> Detail([Required] Guid OrderID)
        {
            ResponseDTO<OrderDetailDTO?> response = await _service.Detail(OrderID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{OrderID}")]
        public async Task<ResponseDTO<OrderDetailDTO?>> Update([Required] Guid OrderID, [Required] OrderUpdateDTO DTO)
        {
            ResponseDTO<OrderDetailDTO?> response = await _service.Update(OrderID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
