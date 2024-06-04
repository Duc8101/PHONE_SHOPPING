using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.IService;
using System.Net;

namespace MVC.Controllers
{
    public class ManagerCategoryController : BaseController
    {
        private readonly IManagerCategoryService _service;

        public ManagerCategoryController(IManagerCategoryService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index(string? name, int? page)
        {
            ResponseDTO response = await _service.Index(name, page);
            if (response.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, response.Message, response.Code));
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["result"] = response.Data;
            dic["name"] = name == null ? "" : name.Trim();
            return View(dic);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryCreateUpdateDTO DTO)
        {
            ResponseDTO response = await _service.Create(DTO);

            if(response.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, response.Message, response.Code));
            }

            if ((bool)response.Data == false)
            {
                ViewData["error"] = response.Message;
                return View();
            }
            ViewData["success"] = response.Message;
            return View();
        }
        public async Task<ActionResult> Update(int? id)
        {
            int? role = getRole();
            if (role == RoleConst.ROLE_ADMIN)
            {
                if (id == null)
                {
                    return Redirect("/ManagerCategory");
                }
                ResponseDTO response = await _service.Update(id.Value);
                if (response.Data == null)
                {
                    if (response.Code == (int)HttpStatusCode.NotFound)
                    {
                        return Redirect("/ManagerCategory");
                    }
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, response.Message, response.Code));
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        [HttpPost]
        public async Task<ActionResult> Update(int id, CategoryCreateUpdateDTO DTO)
        {
            ResponseDTO response = await _service.Update(id, DTO);
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.NotFound)
                {
                    return Redirect("/ManagerCategory");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, response.Message, response.Code));
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
    }
}
