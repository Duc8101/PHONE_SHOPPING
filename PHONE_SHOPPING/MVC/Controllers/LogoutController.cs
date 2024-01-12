using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System.Net;

namespace MVC.Controllers
{
    public class LogoutController : Controller
    {
        private readonly LogoutService service = new LogoutService();
        public async Task<ActionResult> Index()
        {
            HttpContext.Session.Clear();
            string? UserID = Request.Cookies["UserID"];
            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            if (UserID != null)
            {
                Response.Cookies.Append("UserID", UserID, option);
                ResponseDTO<bool> response = await service.Index(UserID);
                if(response.Code == (int) HttpStatusCode.OK)
                {
                    return Redirect("/Home");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
            }
            return Redirect("/Home");
        }
    }
}
