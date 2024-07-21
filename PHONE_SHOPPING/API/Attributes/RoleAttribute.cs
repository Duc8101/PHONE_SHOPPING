using API.Providers;
using Common.Base;
using Common.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using DataAccess.DBContext;
using Common.Enums;

namespace API.Attributes
{
    public class RoleAttribute : Attribute, IActionFilter
    {
        public Roles[] Roles { get; }
        public RoleAttribute(params Roles[] roles)
        {
            Roles = roles;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var accessor = StaticServiceProvider.Provider.GetRequiredService<IHttpContextAccessor>();
            var dbContext = accessor.HttpContext?.RequestServices.GetRequiredService<PHONE_STOREContext>();
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
                else if (!Roles.Contains((Roles)user.RoleId))
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
