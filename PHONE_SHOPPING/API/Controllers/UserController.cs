﻿using API.Attributes;
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
        public ResponseBase Detail([Required] Guid userId)
        {
            ResponseBase response = _service.Detail(userId);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public ResponseBase Login([Required] LoginDTO DTO)
        {
            ResponseBase response = _service.Login(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet]
        [Authorize]
        public ResponseBase Logout()
        {
            ResponseBase response;
            User? user = (User?)HttpContext.Items["user"];
            if (user == null)
            {
                response = new ResponseBase(false, "Not found user id", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.Logout(user.UserId);
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseBase> Create([Required] UserCreateDTO DTO)
        {
            ResponseBase response = await _service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseBase> ForgotPassword(ForgotPasswordDTO DTO)
        {
            ResponseBase response = await _service.ForgotPassword(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut]
        [Role(RoleEnum.Customer)]
        [Authorize]
        public ResponseBase Update([Required] UserUpdateDTO DTO)
        {
            User? user = (User?) HttpContext.Items["user"];
            ResponseBase response;
            if (user == null)
            {
                response = new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.Update(user, DTO);
            }   
            Response.StatusCode = response.Code;
            return response;
        }


        [HttpPut]
        [Authorize]
        public ResponseBase ChangePassword([Required] ChangePasswordDTO DTO)
        {
            User? user = (User?)HttpContext.Items["user"];
            ResponseBase response;
            if (user == null)
            {
                response = new ResponseBase(false, "Not found user", (int)HttpStatusCode.NotFound);
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
