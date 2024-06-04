using MVC.Services.IService;
using MVC.Services.Service;

namespace MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
            builder.Services.AddScoped<IHomeService, HomeService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<ILogoutService, LogoutService>();
            /*            builder.Services.AddScoped<ICartService, CartService>();
                        builder.Services.AddScoped<IChangePasswordService, ChangePasswordService>();
                        builder.Services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
            */            //builder.Services.AddScoped<IHomeService, HomeService>();
            /*            
                        
                        builder.Services.AddScoped<IManagerCategoryService, ManagerCategoryService>();
                        builder.Services.AddScoped<IManagerOrderService, ManagerOrderService>();
                        builder.Services.AddScoped<IManagerProductService, ManagerProductService>();
                        builder.Services.AddScoped<IMyOrderService, MyOrderService>();
                        builder.Services.AddScoped<IProfileService, ProfileService>();
                        builder.Services.AddScoped<IRegisterService, RegisterService>();*/
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
