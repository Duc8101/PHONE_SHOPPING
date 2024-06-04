using API.Attributes;
using API.Services.IService;
using DataAccess.Base;
using DataAccess.DTO.CartDTO;
using DataAccess.Entity;
using DataAccess.Enum;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet]
        [Role<List<CartListDTO>>(RoleEnum.Customer)]
        [Authorize<List<CartListDTO>>]
        public async Task<ResponseBase<List<CartListDTO>?>> List()
        {
            User? user = (User?)HttpContext.Items["user"];
            ResponseBase<List<CartListDTO>?> response;
            if (user == null)
            {
                response = new ResponseBase<List<CartListDTO>?>(null, "Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = await _service.List(user.UserId);
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        [Role<bool>(RoleEnum.Customer)]
        [Authorize<bool>]

        public async Task<ResponseBase<bool>> Create([Required] CartCreateRemoveDTO DTO)
        {
            User? user = (User?)HttpContext.Items["user"];
            ResponseBase<bool> response;
            if (user == null)
            {
                response = new ResponseBase<bool>(false, "Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = await _service.Create(DTO, user.UserId);
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        [Role<bool>(RoleEnum.Customer)]
        [Authorize<bool>]

        public async Task<ResponseBase<bool>> Remove([Required] CartCreateRemoveDTO DTO)
        {
            User? user = (User?)HttpContext.Items["user"];
            ResponseBase<bool> response;
            if (user == null)
            {
                response = new ResponseBase<bool>(false, "Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = await _service.Remove(DTO, user.UserId);
            }
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
