using Common.Base;
using Common.Const;
using Common.DTO.OrderDetailDTO;
using Common.DTO.OrderDTO;
using Common.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.ManagerOrder;
using MVC.Token;
using System.Net;

namespace MVC.Controllers
{
    //[ResponseCache(NoStore = true)]
    public class ManagerOrderController : BaseController
    {
        private readonly IManagerOrderService _service;

        public ManagerOrderController(IManagerOrderService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index(string? status, int? page)
        {
            /*if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
            ResponseBase<Dictionary<string, object>?> response = await _service.Index(status, page);
            if (response.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            return View(response.Data);
        }
        public async Task<ActionResult> View(Guid? id)
        {
            /*if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
            if (id == null)
            {
                return Redirect("/ManagerOrder");
            }
            ResponseBase<UserDetailDTO?> response = await _service.View(id.Value);
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.NotFound)
                {
                    return Redirect("/ManagerOrder");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            return View(response.Data);
        }
        public async Task<ActionResult> Detail(Guid? id)
        {
           /* if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
            if (id == null)
            {
                return Redirect("/ManagerOrder");
            }
            ResponseBase<OrderDetailDTO?> response = await _service.Detail(id.Value);
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.NotFound)
                {
                    return Redirect("/ManagerOrder");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            return View(response.Data);
        }
        public async Task<ActionResult> Update(Guid? id)
        {
/*            if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
            if (id == null)
            {
                return Redirect("/ManagerOrder");
            }
            ResponseBase<OrderDetailDTO?> response = await _service.Detail(id.Value);
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.NotFound)
                {
                    return Redirect("/ManagerOrder");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            if (response.Data.Status == OrderConst.STATUS_APPROVED || response.Data.Status == OrderConst.STATUS_REJECTED)
            {
                return Redirect("/ManagerOrder");
            }
            return View(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> Update(Guid id, OrderUpdateDTO DTO)
        {
/*            if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
            ResponseBase<OrderDetailDTO?> response = await _service.Update(id, DTO);
            if (response.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
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
