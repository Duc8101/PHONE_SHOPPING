using Common.Base;
using Common.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.Register;
using System.Net;

namespace MVC.Controllers
{
    public class RegisterController : BaseController
    {
        private readonly IRegisterService _service;

        public RegisterController(IRegisterService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            int? role = getRole();
            if (role == null)
            {
                return View();
            }
            return Redirect("/Home");
        }

        [HttpPost]
        public async Task<ActionResult> Index(UserCreateDTO DTO)
        {
            ResponseBase<bool> response = await _service.Index(DTO);
            if (response.Code == (int)HttpStatusCode.OK)
            {
                ViewData["success"] = response.Message;
                return View();
            }
            if (response.Code == (int)HttpStatusCode.Conflict)
            {
                ViewData["error"] = response.Message;
                return View();
            }
            return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
        }
    }
}
