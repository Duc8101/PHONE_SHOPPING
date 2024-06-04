using DataAccess.Base;
using DataAccess.DTO.CategoryDTO;
using DataAccess.Pagination;
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
            ResponseBase<Pagination<CategoryListDTO>?> response = await _service.Index(name, page);
            if (response.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
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
            ResponseBase<bool> response = await _service.Create(DTO);

            if (response.Data == false)
            {
                if (response.Code == (int)HttpStatusCode.InternalServerError)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
                }
                ViewData["error"] = response.Message;
                return View();
            }
            ViewData["success"] = response.Message;
            return View();
        }
        public async Task<ActionResult> Update(int? id)
        {
            if (id == null)
            {
                return Redirect("/ManagerCategory");
            }
            ResponseBase<CategoryListDTO?> response = await _service.Update(id.Value);
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.NotFound)
                {
                    return Redirect("/ManagerCategory");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            return View(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> Update(int id, CategoryCreateUpdateDTO DTO)
        {
            ResponseBase<CategoryListDTO?> response = await _service.Update(id, DTO);
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.NotFound)
                {
                    return Redirect("/ManagerCategory");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
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
