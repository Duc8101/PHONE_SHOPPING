using Common.Base;
using Common.DTO.CategoryDTO;
using Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.ManagerCategory;
using System.Net;

namespace MVC.Controllers
{
    //[ResponseCache(NoStore = true)]
    public class ManagerCategoryController : BaseController
    {
        private readonly IManagerCategoryService _service;

        public ManagerCategoryController(IManagerCategoryService service)
        {
            _service = service;
        }

        public async Task<ActionResult> Index(string? name, int? page)
        {
            /*if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
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
            /*if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryCreateUpdateDTO DTO)
        {
            /* if (StaticToken.Token == null)
             {
                 return Redirect("/Home");
             }*/
            ResponseBase<bool?> response = await _service.Create(DTO);
            if (response.Data == false || response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.Conflict)
                {
                    ViewData["error"] = response.Message;
                    return View();
                }
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            ViewData["success"] = response.Message;
            return View();
        }
        public async Task<ActionResult> Update(int? id)
        {
            /*if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
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
            /* if (StaticToken.Token == null)
             {
                 return Redirect("/Home");
             }*/
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
