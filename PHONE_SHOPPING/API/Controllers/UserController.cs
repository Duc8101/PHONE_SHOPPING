using API.Attributes;
using API.Services.Users;
using Common.Base;
using Common.DTO.UserDTO;
using Common.Entity;
using Common.Enum;
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

        [HttpGet("{userId}")]
        public ResponseBase<UserDetailDTO?> Detail([Required] Guid userId)
        {
            ResponseBase<UserDetailDTO?> response = _service.Detail(userId);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public ResponseBase<UserDetailDTO?> Login([Required] LoginDTO DTO)
        {
            ResponseBase<UserDetailDTO?> response = _service.Login(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet]
        [Authorize<bool>]
        public ResponseBase<bool> Logout()
        {
            ResponseBase<bool> response;
            User? user = (User?)HttpContext.Items["user"];
            if (user == null)
            {
                response = new ResponseBase<bool>(false, "Not found user id", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.Logout(user.UserId);
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseBase<bool>> Create([Required] UserCreateDTO DTO)
        {
            ResponseBase<bool> response = await _service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseBase<bool>> ForgotPassword(ForgotPasswordDTO DTO)
        {
            ResponseBase<bool> response = await _service.ForgotPassword(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut]
        [Role<UserDetailDTO>(RoleEnum.Customer)]
        [Authorize<UserDetailDTO>]
        public ResponseBase<UserDetailDTO?> Update([Required] UserUpdateDTO DTO)
        {
            User? user = (User?) HttpContext.Items["user"];
            ResponseBase<UserDetailDTO?> response;
            if (user == null)
            {
                response = new ResponseBase<UserDetailDTO?>(null, "Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.Update(user, DTO);
            }   
            Response.StatusCode = response.Code;
            return response;
        }


        [HttpPut]
        [Authorize<bool>]
        public ResponseBase<bool> ChangePassword([Required] ChangePasswordDTO DTO)
        {
            User? user = (User?)HttpContext.Items["user"];
            ResponseBase<bool> response;
            if (user == null)
            {
                response = new ResponseBase<bool>(false, "Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                 response = _service.ChangePassword(user, DTO);
            }            
            Response.StatusCode = response.Code;
            return response;
        }

    }
}
