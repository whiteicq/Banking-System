using BusinessLogicLayer.Interfaces;
using DataLayer.EF;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Authentificate(string nickname, string password)
        {
            if (_authService.Authentificate(nickname, password))
            {
                int userId = _db.Accounts.FirstOrDefault(a => a.UserName == nickname)!.Id;
                string token = _authService.GenerateToken(userId);
                /*return Ok(new { token });*/
                return Authorization(token, userId);
            }

            return Unauthorized();
        }

        [HttpGet]
        public IActionResult Authorization(string token, int userId)
        {
            // Получение заголовка "Authorization" из запроса
            /*string token = Request.Headers["Authorization"];*/

            if (!string.IsNullOrEmpty(token))
            {
                // Проверка валидности токена
                bool isValid = _authService.ValidateToken(token);

                if (isValid)
                {
                    // Получение роли пользователя из токена
                    string role = "Client"; // Здесь можно использовать аутентифицированного пользователя для получения его роли

                    // Проверка авторизации пользователя
                    bool isAuthorized = _authService.Authorize(token, role);

                    if (isAuthorized)
                    {
                        // Возвращаем информацию о пользователе
                        /*return Ok(new { username = User.Identity.Name });*/
                        // тут редирект на главную
                        TempData["userId"] = userId;
                        
                        return RedirectToAction("MyAccount", "Account");
                    }
                }
            }

            return Unauthorized(); // Недействительный токен или недостаточно прав для доступа
        }
    }
}
