using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BusinessLogicLayer.Mappings;
using DataLayer.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text;
using DataLayer.Entities;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.DTOModels;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Banking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(options => 
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });

            /*builder.Services.AddDbContext<BankingDbContext>(s => new BankingDbContext(builder.Configuration["DefaultConnection"]!));*/
            builder.Services.AddDbContext<BankingDbContext>(options => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BankingDb;Trusted_Connection=True;MultipleActiveResultSets=true"));
            builder.Services.AddSingleton(s => new BankingDbContext(builder.Configuration["ConnectionStrings:DefaultConnection"]!));
            builder.Services.AddAutoMapper(typeof(BLLMappingProfile));

            
            builder.Services.AddScoped<AccountDTO>();

            builder.Services.AddSingleton<IAuthService>(authService => new AuthentificationService(new BankingDbContext(builder.Configuration["ConnectionStrings:DefaultConnection"]!), builder.Configuration["Jwt:Key"]!, builder.Configuration));
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IRegistrationService, ClientRegistrationService>();
            /*builder.Services.AddScoped<IFInancialOperations, BankAccountService>();*/

            // Для куки
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new PathString("/LogIn/Login"); // путь перенаправление анонимных пользователей
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Start}/{action=Index}/{id?}");

            app.Run();
        }
    }
}