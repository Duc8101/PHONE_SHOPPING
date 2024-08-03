using Common.Base;
using Common.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.ChangePassword;
using System.Net;

namespace MVC.Controllers
{
    //[ResponseCache(NoStore = true)]
    public class ChangePasswordController : BaseController
    {
        private readonly IChangePasswordService _service;
        public ChangePasswordController(IChangePasswordService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            /*if (StaticToken.Token == null)
            {
                return Redirect("/Home");
            }*/
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(ChangePasswordDTO DTO)
        {
            ResponseBase<bool?> response = await _service.Index(DTO);
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
            return View("/Views/Shared/Error.cshtml", new ResponseBase(response.Message, response.Code));
        }
    }
}
