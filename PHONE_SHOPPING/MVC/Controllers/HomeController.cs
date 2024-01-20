using DataAccess.Const;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System.Net;

namespace MVC.Controllers
{
    public class HomeController : BaseController
    {
        private readonly HomeService service = new HomeService();
        public async Task<ActionResult> Index(string? name, int? CategoryID, int? page)
        {
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                return Redirect("/ManagerProduct");
            }
            ResponseDTO<Dictionary<string, object>?> result = await service.Index(name, CategoryID, page);
            if (result.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, result.Message, result.Code));
            }
            return View(result.Data);
        }

    }
}