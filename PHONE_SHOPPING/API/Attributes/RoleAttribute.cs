﻿using API.Providers;
using Common.Base;
using Common.Entity;
using Common.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using DataAccess.DBContext;

namespace API.Attributes
{
    public class RoleAttribute : Attribute, IActionFilter
    {
        private RoleEnum[] Roles { get; set; }
        public RoleAttribute(params RoleEnum[] roles)
        {
            Roles = roles;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var accessor = StaticServiceProvider.Provider.GetService<IHttpContextAccessor>();
            var dbContext = accessor?.HttpContext?.RequestServices.GetService<PhoneShoppingContext>();
            if (dbContext == null)
            {
                ResponseBase response = new ResponseBase("Something wrong when check role", (int)HttpStatusCode.InternalServerError);
                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
            }
            else
            {
                // ---------------------- get token -----------------------------
                string? token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                // ---------------------- get user from token -------------------
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadJwtToken(token);
                string userId = tokenS.Claims.First(c => c.Type == "id").Value;
                User? user = dbContext.Users.Find(Guid.Parse(userId));
                if (user == null)
                {
                    ResponseBase response = new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
                    context.Result = new JsonResult(response)
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                    };
                }
                else if (!Roles.Contains((RoleEnum)user.RoleId))
                {
                    ResponseBase response = new ResponseBase("You are not allowed to access", (int)HttpStatusCode.Forbidden);
                    context.Result = new JsonResult(response)
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden,
                    };
                }
            }
        }
    }
}
