using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
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
            int? role = getRole();
            if (role == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(ChangePasswordDTO DTO)
        {
            int? role = getRole();
            if (role == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
            }
            string? UserID = getUserID();
            if (UserID == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, "Not found id. Please check login information", (int)HttpStatusCode.NotFound));
            }
            ResponseDTO response = await _service.Index(UserID, DTO);
            if ((bool?)response.Data == false)
            {
                if (response.Code == (int)HttpStatusCode.Conflict)
                {
                    ViewData["error"] = response.Message;
                    return View();
                }
                return View("/Views/Shared/Error.cshtml", new ResponseDTO(null, response.Message, response.Code));
            }
            ViewData["success"] = response.Message;
            return View();
        }
    }
}
