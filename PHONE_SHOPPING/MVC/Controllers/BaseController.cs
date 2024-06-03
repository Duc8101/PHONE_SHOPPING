using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class BaseController : Controller
    {
        internal int? getRole()
        {
            return HttpContext.Session.GetInt32("role");
        }

        internal string? getUserID()
        {
            return HttpContext.Session.GetString("UserID");
        }
    }
}
