using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System.Net;

namespace MVC.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly ProfileService service = new ProfileService();
        public async Task<ActionResult> Index()
        {
            int? role = getRole();
            if (role == RoleConst.ROLE_CUSTOMER)
            {
                string? UserID = getUserID();
                if (UserID == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "Not found id. Please check login information", (int)HttpStatusCode.NotFound));
                }
                ResponseDTO<UserDetailDTO?> response = await service.Index(UserID);
                // if get user failed
                if (response.Data == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
                }
                return View(response.Data);
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }

        [HttpPost]
        public async Task<ActionResult> Index(UserUpdateDTO DTO)
        {
            int? role = getRole();
            if (role == RoleConst.ROLE_CUSTOMER)
            {
                string? UserID = getUserID();
                if (UserID == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "Not found id. Please check login information", (int)HttpStatusCode.NotFound));
                }
                ResponseDTO<UserDetailDTO?> response = await service.Index(UserID, DTO);
                if (response.Data == null)
                {
                    return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
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
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, "You are not allowed to access this page", (int)HttpStatusCode.Forbidden));
        }
    }
}
