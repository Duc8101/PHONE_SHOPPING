using DataAccess.Const;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.IService;
using System.Net;

namespace MVC.Controllers
{
    public class MyOrderController : BaseController
    {
        private readonly IMyOrderService _service;
        public MyOrderController(IMyOrderService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index(int? page)
        {
            int? role = getRole();
            if (role == RoleConst.ROLE_CUSTOMER)
            {
                string? UserID = getUserID();
                if (UserID == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, "Not found id. Please check login information", (int)HttpStatusCode.NotFound));
                }
                ResponseDTO response = await _service.Index(UserID, page);
                if (response.Data == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, response.Message, response.Code));
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        public async Task<ActionResult> Detail(Guid? id)
        {
            int? role = getRole();
            if (role == RoleConst.ROLE_CUSTOMER)
            {
                string? UserID = getUserID();
                if (UserID == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, "Not found id. Please check login information", (int)HttpStatusCode.NotFound));
                }
                if (id == null)
                {
                    return Redirect("/MyOrder");
                }
                ResponseDTO response = await _service.Detail(id.Value, Guid.Parse(UserID));
                if (response.Data == null)
                {
                    if (response.Code == (int)HttpStatusCode.NotFound)
                    {
                        return Redirect("/MyOrder");
                    }
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, response.Message, response.Code));
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

    }
}
