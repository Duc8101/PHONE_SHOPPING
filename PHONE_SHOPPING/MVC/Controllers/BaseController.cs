using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class BaseController : Controller
    {
        internal static Guid? IDLogin = null;

        internal int? getRole()
        {
            return HttpContext.Session.GetInt32("role");
        }

        internal string? getUserID()
        {
            return HttpContext.Session.GetString("UserID");
        }

        internal bool isSessionTimeout()
        {
            int? role = getRole();
            return IDLogin.HasValue && role == null;
        }
    }
}
