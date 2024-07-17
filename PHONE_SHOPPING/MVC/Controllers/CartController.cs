using Common.Base;
using Common.DTO.CartDTO;
using Common.DTO.OrderDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.Cart;
using System.Net;

namespace MVC.Controllers
{
    //[ResponseCache(NoStore = true)]
    public class CartController : BaseController
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index()
        {
            /* if (StaticToken.Token == null)
             {
                 return Redirect("/Home");
             }*/
            ResponseBase<List<CartListDTO>?> response = await _service.Index();
            if (response.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            return View(response.Data);
        }
        public async Task<ActionResult> Create(Guid? productId)
        {
            /*if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
            if (productId == null)
            {
                return Redirect("/Home");
            }
            CartCreateDTO DTO = new CartCreateDTO()
            {
                ProductId = productId.Value,
            };
            ResponseBase<bool?> response = await _service.Create(DTO);
            if (response.Data == true)
            {
                return Redirect("/Cart");
            }
            return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
        }

        public async Task<ActionResult> Remove(Guid? productId)
        {
            /*if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
            if (productId == null)
            {
                return Redirect("/Home");
            }
            ResponseBase<bool?> response = await _service.Remove(productId.Value);
            if (response.Data == true)
            {
                return Redirect("/Cart");
            }
            return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
        }

        public async Task<ActionResult> Checkout()
        {
            /* if (StaticToken.Token == null)
             {
                 return Redirect("/Home");
             }*/
            return await Index();
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(OrderCreateDTO DTO)
        {
            /*if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
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
