using Common.Base;
using Microsoft.AspNetCore.Mvc;
using MVC.Configuration;
using MVC.Services.Logout;
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
            Response.Cookies.Delete("info");
            WebConfig.Token = null;
            WebConfig.IsLogin = false;
            ResponseBase<bool?> response = await _service.Index();
            if (response.Code == (int)HttpStatusCode.OK)
            {      
                return Redirect("/Home");
            }
            return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
        }
    }
}
