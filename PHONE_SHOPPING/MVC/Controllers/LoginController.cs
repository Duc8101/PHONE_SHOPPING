using DataAccess.DTO.UserDTO;
using DataAccess.DTO;
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
            ResponseDTO<UserDTO?> response = await service.Index(UserID);
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
            return View();
        }
    }
}
