using API.Attributes;
using API.Services.IService;
using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using DataAccess.Entity;
using DataAccess.Enum;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

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
        public async Task<ResponseDTO> Detail([Required] Guid UserID)
        {
            ResponseDTO response = await _service.Detail(UserID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO> Login([Required] LoginDTO DTO)
        {
            ResponseDTO response = await _service.Login(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet]
        [Authorize]
        public async Task<ResponseDTO> Logout()
        {
            ResponseDTO response;
            User? user = (User?)HttpContext.Items["user"];
            if(user == null)
            {
                response = new ResponseDTO(false, "Not found user id", (int) HttpStatusCode.NotFound);
            }
            else
            {
                response = await _service.Logout(user.UserId);
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO> Create([Required] UserCreateDTO DTO)
        {
            ResponseDTO response = await _service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO> ForgotPassword(ForgotPasswordDTO DTO)
        {
            ResponseDTO response = await _service.ForgotPassword(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{UserID}")]
        [Role(RoleEnum.Customer)]
        [Authorize]
        public async Task<ResponseDTO> Update([Required] Guid UserID, [Required] UserUpdateDTO DTO)
        {
            ResponseDTO response = await _service.Update(UserID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{UserID}")]
        [Authorize]
        public async Task<ResponseDTO> ChangePassword([Required] Guid UserID, [Required] ChangePasswordDTO DTO)
        {
            ResponseDTO response = await _service.ChangePassword(UserID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
