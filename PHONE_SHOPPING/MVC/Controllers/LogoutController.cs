using DataAccess.Base;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.IService;
using MVC.Token;
using System.Net;

namespace MVC.Controllers
{
    public class LogoutController : BaseController
    {
        private readonly ILogoutService _service;

        public LogoutController(ILogoutService service)
        {
            _service = service;
        }
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
                ResponseBase<bool> response = await _service.Index();
                if (response.Code == (int)HttpStatusCode.OK)
                {
                    return Redirect("/Home");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            StaticToken.Token = null;
            return Redirect("/Home");
        }
    }
}
