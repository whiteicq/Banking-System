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
    public class LoginController : Controller
    {
        IAuthService _authService;
        IEmailService _emailService;
        BankingDbContext _db;
        public LoginController(IAuthService authService, IEmailService emailService, BankingDbContext db)
        {
            _authService = authService;
            _emailService = emailService;
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string nickname, string password, string authCode)
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

            SignInUser(account);

            return RedirectToAction("MyAccount", "Account", account);    
            
        }

        [HttpGet]
        public IActionResult AuthCode()
        {
            return View();
        }

        /*[HttpPost]
        public IActionResult AuthCode()
        {
            _emailService.
        }*/

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private async void SignInUser(Account account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.Role, account.Role.ToString())
            };

            // создаем объект ClaimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }

    }
}
