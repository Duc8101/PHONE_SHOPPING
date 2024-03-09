using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System.Net;

namespace MVC.Controllers
{
    public class ManagerCategoryController : BaseController
    {
        private readonly ManagerCategoryService _service;

        public ManagerCategoryController(ManagerCategoryService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index(string? name, int? page)
        {
            // if session time out
            if (isSessionTimeout())
            {
                return Redirect("/Logout");
            }
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                ResponseDTO<PagedResultDTO<CategoryListDTO>?> response = await _service.Index(name, page);
                if(response.Data == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["result"] = response.Data;
                dic["name"] = name == null ? "" : name.Trim();
                return View(dic);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        public ActionResult Create()
        {
            // if session time out
            if (isSessionTimeout())
            {
                return Redirect("/Logout");
            }
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                return View();
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryCreateUpdateDTO DTO)
        {
            // if session time out
            if (isSessionTimeout())
            {
                return Redirect("/Logout");
            }
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                ResponseDTO<bool> response = await _service.Create(DTO);
                if(response.Data == false)
                {
                    if(response.Code == (int) HttpStatusCode.Conflict)
                    {
                        ViewData["error"] = response.Message; 
                        return View();
                    }
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                ViewData["success"] = response.Message;
                return View();
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }
        public async Task<ActionResult> Update(int? id)
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
                    return Redirect("/ManagerCategory");
                }
                ResponseDTO<CategoryListDTO?> response = await _service.Update(id.Value);
                if(response.Data == null)
                {
                    if(response.Code == (int) HttpStatusCode.NotFound)
                    {
                        return Redirect("/ManagerCategory");
                    }
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        [HttpPost]
        public async Task<ActionResult> Update(int id, CategoryCreateUpdateDTO DTO)
        {
            // if session time out
            if (isSessionTimeout())
            {
                return Redirect("/Logout");
            }
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                ResponseDTO<CategoryListDTO?> response = await _service.Update(id, DTO);
                if (response.Data == null)
                {
                    if (response.Code == (int)HttpStatusCode.NotFound)
                    {
                        return Redirect("/ManagerCategory");
                    }
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                if(response.Code == (int) HttpStatusCode.Conflict)
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
    }
}
