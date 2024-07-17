using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class BaseController : Controller
    {
        internal int? getRole()
        {
            return HttpContext.Session.GetInt32("role");
        }

        internal string? getUserId()
        {
            return HttpContext.Session.GetString("userId");
        }
    }
}
