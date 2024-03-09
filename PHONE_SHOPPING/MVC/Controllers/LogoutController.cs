using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System.Net;

namespace MVC.Controllers
{
    public class LogoutController : BaseController
    {
        private readonly LogoutService _service;

        public LogoutController(LogoutService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index()
        {
            if (IDLogin.HasValue)
            {
                ResponseDTO<bool> response = await _service.Index(IDLogin.Value);
                if (response.Code == (int)HttpStatusCode.OK)
                {
                    HttpContext.Session.Clear();
                    IDLogin = null;
                    return Redirect("/Home");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
            }
            HttpContext.Session.Clear();
            /*            string? UserID = Request.Cookies["UserID"];
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
            */
            IDLogin = null;
            return Redirect("/Home");
        }
    }
}
