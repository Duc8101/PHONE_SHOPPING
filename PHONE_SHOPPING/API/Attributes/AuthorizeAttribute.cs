using Common.Base;
using Common.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace API.Attributes
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            User? user = (User?)context.HttpContext.Items["user"];
            if (user == null)
            {
                ResponseBase response = new ResponseBase("Unauthorized", (int)HttpStatusCode.Unauthorized);
                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                };
            }
        }
    }
}
