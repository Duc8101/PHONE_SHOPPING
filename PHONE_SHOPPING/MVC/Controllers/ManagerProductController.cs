using Common.Base;
using Common.DTO.CategoryDTO;
using Common.DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.ManagerProduct;
using System.Net;

namespace MVC.Controllers
{
    //[ResponseCache(NoStore = true)]
    public class ManagerProductController : BaseController
    {
        private readonly IManagerProductService _service;

        public ManagerProductController(IManagerProductService service)
        {
            _service = service;
        }

        public async Task<ActionResult> Index(string? name, int? categoryId, int? page)
        {
            /*            if(StaticToken.Token == null)
                        {
                            return Redirect("/Home");
                        }*/
            ResponseBase<Dictionary<string, object>?> result = await _service.Index(name, categoryId, page);
            if (result.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, result.Message, result.Code));
            }
            return View(result.Data);
        }

        public async Task<ActionResult> Create()
        {
            /*            if (StaticToken.Token == null)
                        {
                            return Redirect("/Home");
                        }*/
            ResponseBase<List<CategoryListDTO>?> response = await _service.Create();
            if (response.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            return View(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductCreateUpdateDTO DTO)
        {
            /*            if (StaticToken.Token == null)
                        {
                            return Redirect("/Home");
                        }*/
            ResponseBase<List<CategoryListDTO>?> response = await _service.Create(DTO);
            if (response.Data == null)
            {
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

        public async Task<ActionResult> Update(Guid? id)
        {
            /*if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
            if (id == null)
            {
                return Redirect("/ManagerProduct");
            }
            ResponseBase<Dictionary<string, object>?> response = await _service.Update(id.Value);
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.NotFound)
                {
                    return Redirect("/ManagerProduct");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            return View(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> Update(Guid id, ProductCreateUpdateDTO DTO)
        {
            /*if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
            ResponseBase<Dictionary<string, object>?> response = await _service.Update(id, DTO);
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.NotFound)
                {
                    return Redirect("/ManagerProduct");
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

        public async Task<ActionResult> Delete(Guid? id)
        {
            /*  if (StaticToken.Token == null)
              {
                  return Redirect("/Home");
              }*/
            if (id == null)
            {
                return Redirect("/ManagerProduct");
            }
            ResponseBase<Dictionary<string, object>?> response = await _service.Delete(id.Value);
            if (response.Data == null)
            {
                if (response.Code == (int)HttpStatusCode.NotFound)
                {
                    return Redirect("/ManagerProduct");
                }
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            ViewData["message"] = response.Message;
            return View("/Views/ManagerProduct/Index.cshtml", response.Data);
        }
    }
}
