using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.OrderDetailDTO;
using DataAccess.DTO.OrderDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.IService;
using System.Net;

namespace MVC.Controllers
{
    public class ManagerOrderController : BaseController
    {
        private readonly IManagerOrderService _service;

        public ManagerOrderController(IManagerOrderService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index(string? status, int? page)
        {
            ResponseDTO response = await _service.Index(status, page);
            if (response.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, response.Message, response.Code));
            }
            return View(response.Data);
        }

        public async Task<ActionResult> View(Guid? id)
        {
            if (id == null)
            {
                return Redirect("/ManagerOrder");
            }
            ResponseDTO response = await _service.View(id.Value);
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.NotFound)
                {
                    return Redirect("/ManagerOrder");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, response.Message, response.Code));
            }
            return View(response.Data);
        }

        public async Task<ActionResult> Detail(Guid? id)
        {
            if (id == null)
            {
                return Redirect("/ManagerOrder");
            }
            ResponseDTO response = await _service.Detail(id.Value);
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.NotFound)
                {
                    return Redirect("/ManagerOrder");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, response.Message, response.Code));
            }
            return View(response.Data);
        }

        public async Task<ActionResult> Update(Guid? id)
        {
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                if (id == null)
                {
                    return Redirect("/ManagerOrder");
                }
                ResponseDTO response = await _service.Detail(id.Value);
                if (response.Data == null)
                {
                    if (response.Code == (int)HttpStatusCode.NotFound)
                    {
                        return Redirect("/ManagerOrder");
                    }
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, response.Message, response.Code));
                }
                if (((OrderDetailDTO)response.Data).Status == OrderConst.STATUS_APPROVED || ((OrderDetailDTO)response.Data).Status == OrderConst.STATUS_REJECTED)
                {
                    return Redirect("/ManagerOrder");
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        [HttpPost]
        public async Task<ActionResult> Update(Guid id, OrderUpdateDTO DTO)
        {
            ResponseDTO response = await _service.Update(id, DTO);
            if (response.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, response.Message, response.Code));
            }
            if (response.Code == (int)HttpStatusCode.OK)
            {
                ViewData["success"] = response.Message;
            }
            else
            {
                ViewData["error"] = response.Message;
            }
            return View(response.Data);
        }
    }
}
