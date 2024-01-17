using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO.OrderDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System.Net;

namespace MVC.Controllers
{
    public class CartController : BaseController
    {
        private readonly CartService service = new CartService();
        public async Task<ActionResult> Index()
        {
            int? role = getRole();
            if (role == RoleConst.ROLE_CUSTOMER)
            {
                string? UserID = getUserID();
                if (UserID == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "Not found ID. Please check login information", (int)HttpStatusCode.NotFound));
                }
                ResponseDTO<List<CartListDTO>?> response = await service.Index(UserID);
                if (response.Data == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        public async Task<ActionResult> Create(Guid? ProductID)
        {
            int? role = getRole();
            if (role == RoleConst.ROLE_CUSTOMER)
            {
                string? UserID = getUserID();
                if (UserID == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "Not found ID. Please check login information", (int)HttpStatusCode.NotFound));
                }
                if (ProductID == null)
                {
                    return Redirect("/Home");
                }
                CartCreateRemoveDTO DTO = new CartCreateRemoveDTO()
                {
                    ProductId = ProductID.Value,
                    UserId = Guid.Parse(UserID)
                };
                ResponseDTO<bool> response = await service.Create(DTO);
                if (response.Data)
                {
                    return Redirect("/Cart");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        public async Task<ActionResult> Remove(Guid? ProductID)
        {
            int? role = getRole();
            if (role == RoleConst.ROLE_CUSTOMER)
            {
                string? UserID = getUserID();
                if (UserID == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "Not found ID. Please check login information", (int)HttpStatusCode.NotFound));
                }
                if (ProductID == null)
                {
                    return Redirect("/Home");
                }
                CartCreateRemoveDTO DTO = new CartCreateRemoveDTO()
                {
                    ProductId = ProductID.Value,
                    UserId = Guid.Parse(UserID)
                };
                ResponseDTO<bool> response = await service.Remove(DTO);
                if (response.Data || response.Code == (int)HttpStatusCode.Conflict)
                {
                    return Redirect("/Cart");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }
        public async Task<ActionResult> Checkout()
        {
            return await Index();
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(OrderCreateDTO DTO)
        {
            int? role = getRole();
            if (role == RoleConst.ROLE_CUSTOMER)
            {
                string? UserID = getUserID();
                if (UserID == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "Not found ID. Please check login information", (int)HttpStatusCode.NotFound));
                }
                DTO.UserId = Guid.Parse(UserID);
                ViewData["address"] = DTO.Address;
                ResponseDTO<List<CartListDTO>?> response = await service.Checkout(DTO);
                if(response.Data == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                if(response.Code == (int) HttpStatusCode.Conflict)
                {
                    ViewData["error"] = response.Message;
                    ViewData["address"] = null;
                }
                else
                {
                    ViewData["success"] = response.Message;
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }
    }
}
