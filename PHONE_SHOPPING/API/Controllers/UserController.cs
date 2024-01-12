using API.Services;
using DataAccess.DTO.UserDTO;
using DataAccess.DTO;
using DataAccess.Entity;
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
    }
}
