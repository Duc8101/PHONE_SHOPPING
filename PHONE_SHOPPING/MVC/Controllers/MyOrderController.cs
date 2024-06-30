using Common.Base;
using Common.DTO.OrderDetailDTO;
using Common.DTO.OrderDTO;
using Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.MyOrder;
using MVC.Token;
using System.Net;

namespace MVC.Controllers
{
    //[ResponseCache(NoStore = true)]
    public class MyOrderController : BaseController
    {
        private readonly IMyOrderService _service;
        public MyOrderController(IMyOrderService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index(int? page)
        {
/*            if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
            ResponseBase<Pagination<OrderListDTO>?> response = await _service.Index(page);
            if (response.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            return View(response.Data);
        }

        public async Task<ActionResult> Detail(Guid? id)
        {
/*            if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
            string? UserID = getUserID();
            if (UserID == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, "Not found id. Please check login information", (int)HttpStatusCode.NotFound));
            }
            if (id == null)
            {
                return Redirect("/MyOrder");
            }
            ResponseBase<OrderDetailDTO?> response = await _service.Detail(id.Value, Guid.Parse(UserID));
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.NotFound)
                {
                    return Redirect("/MyOrder");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            return View(response.Data);
        }

    }
}
