using DataAccess.Base;
using DataAccess.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.IService;
using MVC.Token;
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
            string? UserID = Request.Cookies["UserID"];
            // if not set cookie or cookie expired
            if (UserID == null)
            {
                return View();
            }
            ResponseBase<UserDetailDTO?> response = await _service.Index(UserID);
            // if get user failed
            if (response.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            HttpContext.Session.SetString("UserID", UserID);
            HttpContext.Session.SetString("username", response.Data.Username);
            HttpContext.Session.SetInt32("role", response.Data.RoleId);
            StaticToken.Token = response.Data.Token;
            return Redirect("/Home");
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginDTO DTO)
        {
            ResponseBase<UserDetailDTO?> response = await _service.Index(DTO);
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
            StaticToken.Token = response.Data.Token;
            CookieOptions option = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            };
            // add cookie
            Response.Cookies.Append("UserID", response.Data.UserId.ToString(), option);
            return Redirect("/Home");
        }
    }
}
