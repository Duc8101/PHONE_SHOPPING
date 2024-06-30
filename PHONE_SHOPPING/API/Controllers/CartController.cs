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
    [Role(RoleEnum.Customer)]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet]
        public ResponseBase List()
        {
            User? user = (User?)HttpContext.Items["user"];
            ResponseBase response;
            if (user == null)
            {
                response = new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.List(user.UserId);
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public ResponseBase Create([Required] CartCreateDTO DTO)
        {
            User? user = (User?)HttpContext.Items["user"];
            ResponseBase response;
            if (user == null)
            {
                response = new ResponseBase(false, "Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.Create(DTO, user.UserId);
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpDelete]
        public ResponseBase Delete([Required] Guid productId)
        {
            User? user = (User?)HttpContext.Items["user"];
            ResponseBase response;
            if (user == null)
            {
                response = new ResponseBase(false, "Not found user", (int)HttpStatusCode.NotFound);
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
