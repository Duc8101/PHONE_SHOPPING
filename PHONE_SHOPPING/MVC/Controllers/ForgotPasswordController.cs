﻿using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System.Net;

namespace MVC.Controllers
{
    public class ForgotPasswordController : BaseController
    {
        private readonly ForgotPasswordService _service;
        public ForgotPasswordController(ForgotPasswordService service) 
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
        public async Task<ActionResult> Index(ForgotPasswordDTO DTO)
        {
            ResponseDTO<bool> response = await _service.ForgotPassword(DTO);
            if(response.Code == (int)HttpStatusCode.NotFound)
            {
                ViewData["error"] = response.Message;
                return View();
            }
            if (response.Code == (int)HttpStatusCode.OK)
            {
                ViewData["success"] = response.Message;
                return View();
            }
            return View("/Views/Shared/Error.cshtml", new ResponseDTO<object?>(null, response.Message, response.Code));
        }
    }
}
