using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class BaseController : Controller
    {
        protected int? getRole()
        {
            return HttpContext.Session.GetInt32("role");
        }

        protected string? getUsername()
        {
            return HttpContext.Session.GetString("username");
        }

        protected string? getUserID()
        {
            return HttpContext.Session.GetString("UserID");
        }
    }
}
