using Common.Base;
using Common.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.IService;
using System.Net;

namespace MVC.Controllers
{
    public class ChangePasswordController : BaseController
    {
        private readonly IChangePasswordService _service;
        public ChangePasswordController(IChangePasswordService service)
        {
            _service = service;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(ChangePasswordDTO DTO)
        {
            ResponseBase<bool> response = await _service.Index(DTO);
            if (response.Code == (int)HttpStatusCode.Conflict)
            {
                ViewData["error"] = response.Message;
                return View();
            }

            if (response.Code == (int)HttpStatusCode.OK)
            {
                ViewData["success"] = response.Message;
                return View();
            }
            return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
        }
    }
}
