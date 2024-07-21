using Common.Base;
using Common.Enums;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.Home;

namespace MVC.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IHomeService _service;
        public HomeController(IHomeService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index(string? name, int? categoryId, int? page)
        {
            int? role = getRole();
            if (role == (int)Roles.Admin)
            {
                return Redirect("/ManagerProduct");
            }
            ResponseBase<Dictionary<string, object>?> result = await _service.Index(name, categoryId, page);
            if (result.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, result.Message, result.Code));
            }
            return View(result.Data);

        }
    }
}
