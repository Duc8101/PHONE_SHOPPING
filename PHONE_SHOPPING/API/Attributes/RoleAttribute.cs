using API.Providers;
using DataAccess.DTO;
using DataAccess.Entity;
using DataAccess.Enum;
using DataAccess.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

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
            var dbContext = accessor?.HttpContext?.RequestServices.GetService<PHONE_SHOPPINGContext>();
            if (dbContext == null)
            {
                ResponseDTO<object?> response = new ResponseDTO<object?>(null, "Something wrong when check role", (int)HttpStatusCode.InternalServerError);
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
                    ResponseDTO<object?> response = new ResponseDTO<object?>(null, "Not found user", (int)HttpStatusCode.NotFound);
                    context.Result = new JsonResult(response)
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                    };
                }
                else if(!Roles.Contains((RoleEnum)user.RoleId))
                {
                    ResponseDTO<object?> response = new ResponseDTO<object?>(null, "You are not allowed to do this operation", (int)HttpStatusCode.Forbidden);
                    context.Result = new JsonResult(response)
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden,
                    };
                }
            }
        }
    }
}
