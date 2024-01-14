using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System.Net;

namespace MVC.Controllers
{
    public class RegisterController : BaseController
    {
        private readonly RegisterService service = new RegisterService();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(RegisterDTO DTO)
        {
            ResponseDTO<bool> response = await service.Register(DTO);
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
