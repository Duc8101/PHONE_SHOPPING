using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.OrderDetailDTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System.Net;

namespace MVC.Controllers
{
    public class ManagerOrderController : BaseController
    {
        private readonly ManagerOrderService _service;

        public ManagerOrderController(ManagerOrderService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index(string? status , int? page)
        {
            // if session time out
            if (isSessionTimeout())
            {
                return Redirect("/Logout");
            }
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                ResponseDTO<Dictionary<string, object>?> response = await _service.Index(status, page);
                if(response.Data == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        public async Task<ActionResult> View(Guid? id)
        {
            // if session time out
            if (isSessionTimeout())
            {
                return Redirect("/Logout");
            }
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                if(id == null)
                {
                    return Redirect("/ManagerOrder");
                }
                ResponseDTO<UserDetailDTO?> response = await _service.View(id.Value);
                if (response.Data == null)
                {
                    if(response.Code == (int) HttpStatusCode.NotFound)
                    {
                        return Redirect("/ManagerOrder");
                    }
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        public async Task<ActionResult> Detail(Guid? id)
        {
            // if session time out
            if (isSessionTimeout())
            {
                return Redirect("/Logout");
            }
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                if (id == null)
                {
                    return Redirect("/ManagerOrder");
                }
                ResponseDTO<OrderDetailDTO?> response = await _service.Detail(id.Value);
                if (response.Data == null)
                {
                    if (response.Code == (int)HttpStatusCode.NotFound)
                    {
                        return Redirect("/ManagerOrder");
                    }
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        public async Task<ActionResult> Update(Guid? id)
        {
            // if session time out
            if (isSessionTimeout())
            {
                return Redirect("/Logout");
            }
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                if (id == null)
                {
                    return Redirect("/ManagerOrder");
                }
                ResponseDTO<OrderDetailDTO?> response = await _service.Detail(id.Value);
                if (response.Data == null)
                {
                    if (response.Code == (int)HttpStatusCode.NotFound)
                    {
                        return Redirect("/ManagerOrder");
                    }
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                if (response.Data.Status == OrderConst.STATUS_APPROVED || response.Data.Status == OrderConst.STATUS_REJECTED)
                {
                    return Redirect("/ManagerOrder");
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        [HttpPost]
        public async Task<ActionResult> Update(Guid id, OrderUpdateDTO DTO)
        {
            // if session time out
            if (isSessionTimeout())
            {
                return Redirect("/Logout");
            }
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                ResponseDTO<OrderDetailDTO?> response = await _service.Update(id, DTO);
                if (response.Data == null) 
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                if(response.Code == (int)HttpStatusCode.OK)
                {
                    ViewData["success"] = response.Message;
                }
                else
                {
                    ViewData["error"] = response.Message;
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }
    }
}
