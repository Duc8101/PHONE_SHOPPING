using API.Attributes;
using API.Services.Carts;
using Common.Base;
using Common.DTO.CartDTO;
using Common.Entity;
using Common.Enum;
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
        public ResponseBase<List<CartListDTO>?> List()
        {
            User? user = (User?)HttpContext.Items["user"];
            ResponseBase<List<CartListDTO>?> response;
            if (user == null)
            {
                response = new ResponseBase<List<CartListDTO>?>(null, "Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.List(user.UserId);
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        [Role<bool>(RoleEnum.Customer)]
        [Authorize<bool>]

        public ResponseBase<bool> Create([Required] CartCreateDTO DTO)
        {
            User? user = (User?)HttpContext.Items["user"];
            ResponseBase<bool> response;
            if (user == null)
            {
                response = new ResponseBase<bool>(false, "Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.Create(DTO, user.UserId);
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpDelete]
        [Role<bool>(RoleEnum.Customer)]
        [Authorize<bool>]

        public ResponseBase<bool> Delete([Required] Guid productId)
        {
            User? user = (User?)HttpContext.Items["user"];
            ResponseBase<bool> response;
            if (user == null)
            {
                response = new ResponseBase<bool>(false, "Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.Delete(productId, user.UserId);
            }
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
