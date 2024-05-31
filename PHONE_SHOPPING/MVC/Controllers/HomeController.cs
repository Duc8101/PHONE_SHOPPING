using DataAccess.Const;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.IService;

namespace MVC.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IHomeService _service;
        public HomeController(IHomeService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index(string? name, int? CategoryID, int? page)
        {
            // if session time out
            if (isSessionTimeout())
            {
                return Redirect("/Logout");
            }
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                return Redirect("/ManagerProduct");
            }
            ResponseDTO<Dictionary<string, object>?> result = await _service.Index(name, CategoryID, page);
            if (result.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, result.Message, result.Code));
            }
            return View(result.Data);
        }

    }
}