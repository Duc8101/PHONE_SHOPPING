using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class BaseController : Controller
    {
        protected int? getRole()
        {
            return HttpContext.Session.GetInt32("role");
        }
    }
}
