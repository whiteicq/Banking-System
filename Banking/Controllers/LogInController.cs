using BusinessLogicLayer.Interfaces;
using DataLayer.EF;
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
            if (_authService.Authenticate(nickname, password))
            {
                string userId = _db.Accounts.FirstOrDefault(a => a.UserName == nickname)!.Id.ToString();
                string token = _authService.GenerateToken(userId);
                return Ok(new { token }); // редирект в аккаунт или на главную
                /*return Authorization();*/
            }

            return Unauthorized();
        }

        [HttpGet]
        public IActionResult Authorization()
        {
            // Получение заголовка "Authorization" из запроса
            string token = Request.Headers["Authorization"];

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
                        return Ok(new { username = User.Identity.Name });
                    }
                }
            }

            return Unauthorized(); // Недействительный токен или недостаточно прав для доступа
        }
    }
}
