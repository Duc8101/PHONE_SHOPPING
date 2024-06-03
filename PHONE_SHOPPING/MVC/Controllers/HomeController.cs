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
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                return Redirect("/ManagerProduct");
            }
            ResponseDTO result = await _service.Index(name, CategoryID, page);
            if (result.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, result.Message, result.Code));
            }
            return View(result.Data);
        }

    }
}