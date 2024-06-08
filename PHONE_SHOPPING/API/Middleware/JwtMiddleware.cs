using DataAccess.DBContext;
using System.IdentityModel.Tokens.Jwt;

namespace API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, PhoneShoppingContext dbContext)
        {
            // get token login
            string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // if get token successful
            if (token != null)
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var tokenS = handler.ReadJwtToken(token);
                    string userId = tokenS.Claims.First(c => c.Type == "id").Value;
                    context.Items["user"] = dbContext.Users.Find(Guid.Parse(userId));
                }
                catch
                {

                }
            }
            await _next(context);
        }
    }
}
