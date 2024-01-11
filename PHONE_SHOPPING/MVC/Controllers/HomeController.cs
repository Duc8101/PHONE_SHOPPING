using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}