using API.Attributes;
using API.Services.IService;
using DataAccess.DTO;
using DataAccess.DTO.CartDTO;
using DataAccess.Enum;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        public async Task<ResponseDTO<List<CartListDTO>?>> List([Required] Guid UserID)
        {
            ResponseDTO<List<CartListDTO>?> response = await _service.List(UserID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO<bool>> Create([Required] CartCreateRemoveDTO DTO)
        {
            ResponseDTO<bool> response = await _service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO<bool>> Remove([Required] CartCreateRemoveDTO DTO)
        {
            ResponseDTO<bool> response = await _service.Remove(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpDelete("{UserID}")]
        public async Task<ResponseDTO<bool>> Delete([Required] Guid UserID)
        {
            ResponseDTO<bool> response = await _service.Delete(UserID);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
