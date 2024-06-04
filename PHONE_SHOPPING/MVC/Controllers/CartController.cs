using DataAccess.Base;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO.OrderDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.IService;
using System.Net;

namespace MVC.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index()
        {
            ResponseBase<List<CartListDTO>?> response = await _service.Index();
            if (response.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            return View(response.Data);
        }

        public async Task<ActionResult> Create(Guid? ProductID)
        {
            if (ProductID == null)
            {
                return Redirect("/Home");
            }
            CartCreateRemoveDTO DTO = new CartCreateRemoveDTO()
            {
                ProductId = ProductID.Value,
            };
            ResponseBase<bool> response = await _service.Create(DTO);
            if (response.Data == true)
            {
                return Redirect("/Cart");
            }
            return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
        }

        public async Task<ActionResult> Remove(Guid? ProductID)
        {
            if (ProductID == null)
            {
                return Redirect("/Home");
            }
            CartCreateRemoveDTO DTO = new CartCreateRemoveDTO()
            {
                ProductId = ProductID.Value,
            };
            ResponseBase<bool> response = await _service.Remove(DTO);
            if (response.Data == true)
            {
                return Redirect("/Cart");
            }
            return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
        }
        public async Task<ActionResult> Checkout()
        {
            return await Index();
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(OrderCreateDTO DTO)
        {
            ViewData["address"] = DTO.Address;
            ResponseBase<List<CartListDTO>?> response = await _service.Checkout(DTO);
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
                ViewData["address"] = null;
            }
            return View(response.Data);
        }
    }
}
