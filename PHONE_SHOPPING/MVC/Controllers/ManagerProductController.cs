using DataAccess.Const;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System.Net;

namespace MVC.Controllers
{
    public class ManagerProductController : BaseController
    {
        private readonly ManagerProductService service = new ManagerProductService();
        public async Task<ActionResult> Index(string? name, int? CategoryID, int? page)
        {
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                ResponseDTO<Dictionary<string, object>?> result = await service.Index(name, CategoryID, page);
                if (result.Code == (int)HttpStatusCode.InternalServerError)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, result.Message, result.Code));
                }
                return View(result.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }
    }
}
