using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System.Net;

namespace MVC.Controllers
{
    public class LoginController : BaseController
    {
        private readonly LoginService service = new LoginService();
        public async Task<ActionResult> Index()
        {
            string? UserID = Request.Cookies["UserID"];
            // if not set cookie or cookie expired
            if (UserID == null)
            {
                return View();
            }
            ResponseDTO<UserDetailDTO?> response = await service.Index(UserID);
            // if get user failed
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.NotFound)
                {
                    return Redirect("/Logout");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
            }
            HttpContext.Session.SetString("UserID", UserID);
            HttpContext.Session.SetString("username", response.Data.Username);
            HttpContext.Session.SetInt32("role", response.Data.RoleId);
            return Redirect("/Home");
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginDTO DTO)
        {
            ResponseDTO<UserDetailDTO?> response = await service.Index(DTO);
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.Conflict)
                {
                    ViewData["message"] = response.Message;
                    return View();
                }
                return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
            }
            HttpContext.Session.SetString("UserID", response.Data.UserId.ToString());
            HttpContext.Session.SetString("username", response.Data.Username);
            HttpContext.Session.SetInt32("role", response.Data.RoleId);
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
