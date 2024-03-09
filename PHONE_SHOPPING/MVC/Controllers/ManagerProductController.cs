using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System.Net;

namespace MVC.Controllers
{
    public class ManagerProductController : BaseController
    {
        private readonly ManagerProductService _service;

        public ManagerProductController(ManagerProductService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index(string? name, int? CategoryID, int? page)
        {
            // if session time out
            if (isSessionTimeout())
            {
                return Redirect("/Logout");
            }
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                ResponseDTO<Dictionary<string, object>?> result = await _service.Index(name, CategoryID, page);
                if (result.Data == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, result.Message, result.Code));
                }
                return View(result.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        public async Task<ActionResult> Create()
        {
            // if session time out
            if (isSessionTimeout())
            {
                return Redirect("/Logout");
            }
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                ResponseDTO<List<CategoryListDTO>?> response = await _service.Create();
                if (response.Data == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductCreateUpdateDTO DTO)
        {
            // if session time out
            if (isSessionTimeout())
            {
                return Redirect("/Logout");
            }
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                ResponseDTO<List<CategoryListDTO>?> response = await _service.Create(DTO);
                if (response.Data == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                if (response.Code == (int)HttpStatusCode.Conflict)
                {
                    ViewData["error"] = response.Message;
                }
                else
                {
                    ViewData["success"] = response.Message;
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
                    return Redirect("/ManagerProduct");
                }
                ResponseDTO<Dictionary<string, object>?> response = await _service.Update(id.Value);
                if (response.Data == null)
                {
                    if (response.Code == (int)HttpStatusCode.NotFound)
                    {
                        return Redirect("/ManagerProduct");
                    }
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        [HttpPost]
        public async Task<ActionResult> Update(Guid id, ProductCreateUpdateDTO DTO)
        {
            // if session time out
            if (isSessionTimeout())
            {
                return Redirect("/Logout");
            }
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                ResponseDTO<Dictionary<string, object>?> response = await _service.Update(id, DTO);
                if (response.Data == null)
                {
                    if (response.Code == (int)HttpStatusCode.NotFound)
                    {
                        return Redirect("/ManagerProduct");
                    }
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                if (response.Code == (int)HttpStatusCode.Conflict)
                {
                    ViewData["error"] = response.Message;
                }
                else
                {
                    ViewData["success"] = response.Message;
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        public async Task<ActionResult> Delete(Guid? id)
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
                    return Redirect("/ManagerProduct");
                }
                ResponseDTO<Dictionary<string, object>?> response = await _service.Delete(id.Value);
                if (response.Data == null)
                {
                    if (response.Code == (int)HttpStatusCode.NotFound)
                    {
                        return Redirect("/ManagerProduct");
                    }
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                ViewData["message"] = response.Message;
                return View("/Views/ManagerProduct/Index.cshtml", response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }
    }
}
