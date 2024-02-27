using DataAccess.DTO.CartDTO;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using API.Services;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService service;
        public CartController(CartService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ResponseDTO<List<CartListDTO>?>> List([Required] Guid UserID)
        {
            ResponseDTO<List<CartListDTO>?> response = await service.List(UserID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO<bool>> Create([Required] CartCreateRemoveDTO DTO)
        {
            ResponseDTO<bool> response = await service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO<bool>> Remove([Required] CartCreateRemoveDTO DTO)
        {
            ResponseDTO<bool> response = await service.Remove(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpDelete("{UserID}")]
        public async Task<ResponseDTO<bool>> Delete([Required] Guid UserID)
        {
            ResponseDTO<bool> response = await service.Delete(UserID);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
