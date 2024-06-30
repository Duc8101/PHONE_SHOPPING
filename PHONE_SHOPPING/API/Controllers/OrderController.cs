using API.Attributes;
using API.Services.Orders;
using Common.Base;
using Common.DTO.OrderDTO;
using Common.Entity;
using Common.Enum;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        [Role(RoleEnum.Customer)]
        public async Task<ResponseBase> Create([Required] OrderCreateDTO DTO)
        {
            User? user = (User?)HttpContext.Items["user"];
            ResponseBase response;
            if (user == null)
            {
                response = new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = await _service.Create(DTO, user.UserId);
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet]
        public ResponseBase List(string? status, [Required] int page = 1)
        {
            ResponseBase response;
            User? user = (User?)HttpContext.Items["user"];
            if (user == null)
            {
                response = new ResponseBase("Not found user id", (int)HttpStatusCode.NotFound);
            }
            else
            {
                bool isAdmin = user.RoleId == (int)RoleEnum.Admin;
                response = _service.List(isAdmin ? null : user.UserId, status, isAdmin, page);
                Response.StatusCode = response.Code;
            }
            return response;
        }

        [HttpGet("{OrderID}")]
        public ResponseBase Detail([Required] Guid OrderID)
        {
            User? user = (User?)HttpContext.Items["user"];
            ResponseBase response;
            if (user == null)
            {
                response = new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            else if(user.RoleId == (int) RoleEnum.Admin)
            {
                response = _service.Detail(OrderID, null);
            }
            else
            {
                response = _service.Detail(OrderID, user.UserId);
            }          
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{OrderID}")]
        [Role(RoleEnum.Admin)]
        public async Task<ResponseBase> Update([Required] Guid OrderID, [Required] OrderUpdateDTO DTO)
        {
            ResponseBase response = await _service.Update(OrderID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
