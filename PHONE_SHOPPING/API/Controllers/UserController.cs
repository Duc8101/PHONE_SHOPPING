using API.Attributes;
using API.Services.IService;
using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using DataAccess.Enum;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("{UserID}")]
        [Role(RoleEnum.Customer)]
        [Authorize]
        public async Task<ResponseDTO<UserDetailDTO?>> Detail([Required] Guid UserID)
        {
            ResponseDTO<UserDetailDTO?> response = await _service.Detail(UserID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO<UserDetailDTO?>> Login([Required] LoginDTO DTO)
        {
            ResponseDTO<UserDetailDTO?> response = await _service.Login(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        /*[HttpGet]
        public async Task<ResponseDTO<bool>> Logout([Required] Guid UserID)
        {
            ResponseDTO<bool> response = await service.Logout(UserID);
            Response.StatusCode = response.Code;
            return response;
        }*/

        [HttpPost]
        public async Task<ResponseDTO<bool>> Create([Required] UserCreateDTO DTO)
        {
            ResponseDTO<bool> response = await _service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO<bool>> ForgotPassword(ForgotPasswordDTO DTO)
        {
            ResponseDTO<bool> response = await _service.ForgotPassword(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{UserID}")]
        [Role(RoleEnum.Customer)]
        [Authorize]
        public async Task<ResponseDTO<UserDetailDTO?>> Update([Required] Guid UserID, [Required] UserUpdateDTO DTO)
        {
            ResponseDTO<UserDetailDTO?> response = await _service.Update(UserID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{UserID}")]
        [Authorize]
        public async Task<ResponseDTO<bool>> ChangePassword([Required] Guid UserID, [Required] ChangePasswordDTO DTO)
        {
            ResponseDTO<bool> response = await _service.ChangePassword(UserID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
