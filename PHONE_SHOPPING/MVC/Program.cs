using MVC.Services;

namespace MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(options =>
                options.IdleTimeout = new TimeSpan(3, 0, 0)
            );
            builder.Services.AddScoped<HttpClient, HttpClient>();
            builder.Services.AddScoped<CartService, CartService>();
            builder.Services.AddScoped<ChangePasswordService, ChangePasswordService>();
            builder.Services.AddScoped<ForgotPasswordService, ForgotPasswordService>();
            builder.Services.AddScoped<HomeService, HomeService>();
            builder.Services.AddScoped<LoginService, LoginService>();
            builder.Services.AddScoped<LogoutService, LogoutService>();
            builder.Services.AddScoped<ManagerCategoryService, ManagerCategoryService>();
            builder.Services.AddScoped<ManagerOrderService, ManagerOrderService>();
            builder.Services.AddScoped<ManagerProductService, ManagerProductService>();
            builder.Services.AddScoped<MyOrderService, MyOrderService>();
            builder.Services.AddScoped<ProfileService, ProfileService>();
            builder.Services.AddScoped<RegisterService, RegisterService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseSession();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

        }
    }
}
