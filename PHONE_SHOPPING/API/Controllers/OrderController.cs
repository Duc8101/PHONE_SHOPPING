using API.Attributes;
using API.Services.IService;
using DataAccess.DTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.Entity;
using DataAccess.Enum;
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
        public async Task<ResponseDTO> Create([Required] OrderCreateDTO DTO)
        {
            ResponseDTO response = await _service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet]
        public async Task<ResponseDTO> List(string? status, [Required] int page = 1)
        {
            ResponseDTO response;
            User? user = (User?)HttpContext.Items["user"];
            if (user == null)
            {
                response = new ResponseDTO(false, "Not found user id", (int)HttpStatusCode.NotFound);
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
        public async Task<ResponseDTO> Detail([Required] Guid OrderID)
        {
            ResponseDTO response = await _service.Detail(OrderID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{OrderID}")]
        [Role(RoleEnum.Admin)]
        public async Task<ResponseDTO> Update([Required] Guid OrderID, [Required] OrderUpdateDTO DTO)
        {
            ResponseDTO response = await _service.Update(OrderID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
