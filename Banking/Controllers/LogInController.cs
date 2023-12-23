using BusinessLogicLayer.Interfaces;
using DataLayer.EF;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.JSInterop.Implementation;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;

namespace Banking.Controllers
{
    public class LogInController : Controller
    {
        IAuthService _authService;
        BankingDbContext _db;
        public LogInController(IAuthService authService, BankingDbContext db)
        {
            _authService = authService;
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string nickname, string password)
        {
            if (!_authService.Authentificate(nickname, password))
            {
                return BadRequest("Invalid user name or password");
            }

            var account = _db.Accounts.FirstOrDefault(a => a.UserName == nickname);
            if (account is null)
            {
                return Unauthorized();
            }

            CreateCookie(account);

            return RedirectToAction("MyAccount", "Account", account);    
            
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private void CreateCookie(Account account)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, account.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Role.ToString())
            };

            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

    }
}
