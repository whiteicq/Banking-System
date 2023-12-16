using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BusinessLogicLayer.Mappings;
using DataLayer.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text;
using DataLayer;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;

namespace Banking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            /*builder.Services.AddDbContext<BankingDbContext>(s => new BankingDbContext(builder.Configuration["DefaultConnection"]!));*/
            builder.Services.AddDbContext<BankingDbContext>(options => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BankingDb;Trusted_Connection=True;MultipleActiveResultSets=true"));
            builder.Services.AddSingleton(s => new BankingDbContext(builder.Configuration["ConnectionStrings:DefaultConnection"]!));
            builder.Services.AddAutoMapper(typeof(BLLMappingProfile));
            builder.Services.AddSingleton<IAuthService>(authService => new AuthentificationService(new BankingDbContext(builder.Configuration["ConnectionStrings:DefaultConnection"]!), builder.Configuration["Jwt:Key"]!));
            /*builder.Services.AddScoped<IAuthService>(authService => new AuthentificationService(new BankingDbContext(builder.Configuration["DefaultConnection"]!), builder.Configuration["Key"]!));*/
            /*builder.Services.AddScoped<IClientService>(clientService => new ClientService()*/
            builder.Services.AddScoped<IRegistrationService, ClientRegistrationService>();
            
            
            // jwt
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });

            builder.Services.AddAuthorization();
            // 

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Start}/{action=Index}/{id?}");

            app.Run();
        }
    }
}