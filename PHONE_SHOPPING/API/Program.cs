using API.Services.IService;
using API.Services.Service;
using AutoMapper;
using DataAccess.Model;
using DataAccess.Model.DAO;
using DataAccess.Model.IDAO;
using Microsoft.EntityFrameworkCore;

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
            // ------------------------- register dbcontext ----------------------------
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<PHONE_SHOPPINGContext>(options =>
                options.UseSqlServer(connection)
            );
            // ------------------------- register service ----------------------------
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddTransient<IDAOUser, DAOUser>();
            builder.Services.AddTransient<IDAOCart, DAOCart>();
            builder.Services.AddTransient<IDAOProduct, DAOProduct>();
            builder.Services.AddTransient<IDAOCategory, DAOCategory>();
            builder.Services.AddTransient<IDAOOrder, DAOOrder>();
            builder.Services.AddTransient<IDAOOrderDetail, DAOOrderDetail>();
            // ------------------------- config auto mapper ----------------------------
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
