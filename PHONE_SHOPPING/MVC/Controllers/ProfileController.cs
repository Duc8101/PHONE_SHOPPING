﻿using Common.Base;
using Common.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.Profile;
using System.Net;

namespace MVC.Controllers
{
    [ResponseCache(NoStore = true)]
    public class ProfileController : BaseController
    {
        private readonly IProfileService _service;

        public ProfileController(IProfileService service)
        {
            _service = service;
        }
        public async Task<ActionResult> Index()
        {
            ResponseBase<UserDetailDTO?> response = await _service.Index();
            // if get user failed
            if (response.Data == null)
            {
                return View("/Views/Shared/Error.cshtml", new ResponseBase<object?>(null, response.Message, response.Code));
            }
            return View(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> Index(UserUpdateDTO DTO)
        {
            /*            if (StaticToken.Token == null)
                        {
                            return Redirect("/Home");
                        }*/
            ResponseBase<UserDetailDTO?> response = await _service.Index(DTO);
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
    }
}
