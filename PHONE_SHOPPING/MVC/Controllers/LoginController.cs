using Common.Base;
using Common.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Configuration;
using MVC.Services.Login;
using System.Net;

namespace MVC.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ILoginService _service;

        public LoginController(ILoginService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index()
        {
            string? token = Request.Cookies["info"];
            if(token == null)
            {
                return View();
            }
            ResponseBase<UserLoginInfoDTO?> response = await _service.Index(token);
            // if get user failed
            if (response.Data == null)
            {
                if(response.Code == (int) HttpStatusCode.Conflict)
                {
                    if (response.Message.Contains("Invalid"))
                    {
                        return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
                    }
                    return View();
                }
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            HttpContext.Session.SetString("UserID", response.Data.UserId.ToString());
            HttpContext.Session.SetString("username", response.Data.Username);
            HttpContext.Session.SetInt32("role", response.Data.RoleId);
            WebConfig.Token = response.Data.Access_Token;
            WebConfig.IsLogin = true;
            return Redirect("/Home");
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginDTO DTO)
        {
            ResponseBase<UserLoginInfoDTO?> response = await _service.Index(DTO);
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.NotFound)
                {
                    ViewData["message"] = response.Message;
                    return View();
                }
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            HttpContext.Session.SetString("UserID", response.Data.UserId.ToString());
            HttpContext.Session.SetString("username", response.Data.Username);
            HttpContext.Session.SetInt32("role", response.Data.RoleId);
            CookieOptions option = new CookieOptions()
            {
                HttpOnly = true,
                Expires = response.Data.ExpireDate,
                Secure = true, // Chỉ sử dụng trên kết nối HTTPS
                SameSite = SameSiteMode.Strict, // Bảo vệ chống lại CSRF
            };
            // add cookie
            Response.Cookies.Append("info", response.Data.Access_Token, option);
            WebConfig.Token = response.Data.Access_Token;
            WebConfig.IsLogin = true;
            return Redirect("/Home");
        }
    }
}
