using Common.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class BaseAPIController : ControllerBase
    {
        internal string? getUserId()
        {
            Claim? claim = User.Claims.Where(c => c.Type == "id").FirstOrDefault();
            return claim?.Value;
        }

        internal bool isAdmin()
        {
            Claim? claim = User.Claims.Where(c => c.Type == "role").FirstOrDefault();
            return claim != null && claim.Value == RoleEnum.Admin.ToString();
        }
    }
}
