using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System.Net;

namespace MVC.Controllers
{
    public class RegisterController : BaseController
    {
        private readonly RegisterService _service;

        public RegisterController(RegisterService service)
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
            ResponseDTO<bool> response = await _service.Register(DTO);
            if(response.Code == (int) HttpStatusCode.OK)
            {
                ViewData["success"] = response.Message;
                return View();
            }
            if (response.Code == (int)HttpStatusCode.Conflict)
            {
                ViewData["error"] = response.Message;
                return View();
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
        }
    }
}
