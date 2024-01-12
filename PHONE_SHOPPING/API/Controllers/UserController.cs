using API.Services;
using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService service;
        public UserController(UserService service)
        {
            this.service = service;
        }

        [HttpGet("{UserID}")]
        public async Task<ResponseDTO<UserDTO?>> Detail([Required] Guid UserID)
        {
            ResponseDTO<UserDTO?> response = await service.Detail(UserID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO<UserDTO?>> Login([Required] LoginDTO DTO)
        {
            ResponseDTO<UserDTO?> response = await service.Login(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet]
        public async Task<ResponseDTO<bool>> Logout([Required] Guid UserID)
        {
            ResponseDTO<bool> response = await service.Logout(UserID);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
