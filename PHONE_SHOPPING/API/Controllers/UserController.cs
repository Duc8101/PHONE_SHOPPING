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
        public async Task<ResponseDTO<UserDetailDTO?>> Detail([Required] Guid UserID)
        {
            ResponseDTO<UserDetailDTO?> response = await service.Detail(UserID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO<UserDetailDTO?>> Login([Required] LoginDTO DTO)
        {
            ResponseDTO<UserDetailDTO?> response = await service.Login(DTO);
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

        [HttpPost]
        public async Task<ResponseDTO<bool>> Create([Required] UserCreateDTO DTO)
        {
            ResponseDTO<bool> response = await service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO<bool>> ForgotPassword(ForgotPasswordDTO DTO)
        {
            ResponseDTO<bool> response = await service.ForgotPassword(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{UserID}")]
        public async Task<ResponseDTO<UserDetailDTO?>> Update([Required] Guid UserID, [Required] UserUpdateDTO DTO)
        {
            ResponseDTO<UserDetailDTO?> response = await service.Update(UserID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{UserID}")]
        public async Task<ResponseDTO<bool>> ChangePassword([Required] Guid UserID, [Required] ChangePasswordDTO DTO)
        {
            ResponseDTO<bool> response = await service.ChangePassword(UserID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
