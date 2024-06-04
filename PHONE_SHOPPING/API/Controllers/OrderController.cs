using API.Attributes;
using API.Services.IService;
using DataAccess.Base;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO.OrderDetailDTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.Entity;
using DataAccess.Enum;
using DataAccess.Pagination;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

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
        [Role<List<CartListDTO>>(RoleEnum.Customer)]
        [Authorize<List<CartListDTO>>]
        public async Task<ResponseBase<List<CartListDTO>?>> Create([Required] OrderCreateDTO DTO)
        {
            User? user = (User?)HttpContext.Items["user"];
            ResponseBase<List<CartListDTO>?> response;
            if (user == null)
            {
                response = new ResponseBase<List<CartListDTO>?>(null, "Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = await _service.Create(DTO, user.UserId);
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet]
        [Authorize<Pagination<OrderListDTO>>]
        public async Task<ResponseBase<Pagination<OrderListDTO>?>> List(string? status, [Required] int page = 1)
        {
            ResponseBase<Pagination<OrderListDTO>?> response;
            User? user = (User?)HttpContext.Items["user"];
            if (user == null)
            {
                response = new ResponseBase<Pagination<OrderListDTO>?>(null, "Not found user id", (int)HttpStatusCode.NotFound);
            }
            else
            {
                bool isAdmin = user.RoleId == (int)RoleEnum.Admin;
                response = await _service.List(isAdmin ? null : user.UserId, status, isAdmin, page);
                Response.StatusCode = response.Code;
            }
            return response;
        }

        [HttpGet("{OrderID}")]
        [Authorize<OrderDetailDTO>]
        public async Task<ResponseBase<OrderDetailDTO?>> Detail([Required] Guid OrderID)
        {
            ResponseBase<OrderDetailDTO?> response = await _service.Detail(OrderID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{OrderID}")]
        [Role<OrderDetailDTO>(RoleEnum.Admin)]
        [Authorize<OrderDetailDTO>]
        public async Task<ResponseBase<OrderDetailDTO?>> Update([Required] Guid OrderID, [Required] OrderUpdateDTO DTO)
        {
            ResponseBase<OrderDetailDTO?> response = await _service.Update(OrderID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
