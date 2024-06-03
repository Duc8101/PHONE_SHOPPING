using DataAccess.DTO.CartDTO;
using DataAccess.DTO;
using DataAccess.Entity;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace API.Attributes
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            User? user = (User?) context.HttpContext.Items["user"];
            if (user == null)
            {
                ResponseDTO<object?> response = new ResponseDTO<object?>(null, "Unauthorized", (int)HttpStatusCode.Unauthorized);
                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                };
            }
        }
    }
}
