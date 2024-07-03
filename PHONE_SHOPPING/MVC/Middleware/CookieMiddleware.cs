using MVC.Configuration;

namespace MVC.Middleware
{
    public class CookieMiddleware
    {
        private readonly RequestDelegate _next;

        public CookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Cookies["info"];
            if (token == null && WebConfig.IsLogin)
            {
                context.Session.Clear();
                context.Response.Redirect("/Login");
            }
            await _next(context);
        }
    }
}
