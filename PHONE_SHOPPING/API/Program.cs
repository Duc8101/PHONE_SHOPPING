using API.Services;
using AutoMapper;
using DataAccess.Model;
using DataAccess.Model.DAO;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ProductService, ProductService>();
            builder.Services.AddScoped<CategoryService, CategoryService>();
            builder.Services.AddScoped<UserService, UserService>();
            builder.Services.AddScoped<CartService, CartService>();
            builder.Services.AddScoped<OrderService, OrderService>();
            builder.Services.AddScoped<PHONE_SHOPPINGContext, PHONE_SHOPPINGContext>();
            builder.Services.AddScoped<DAOUser, DAOUser>();
            builder.Services.AddScoped<DAOCart, DAOCart>();
            builder.Services.AddScoped<DAOProduct, DAOProduct>();
            builder.Services.AddScoped<DAOCategory, DAOCategory>();
            builder.Services.AddScoped<DAOOrder, DAOOrder>();
            builder.Services.AddScoped<DAOOrderDetail, DAOOrderDetail>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}
