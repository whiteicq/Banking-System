using BusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using DataLayer.EF;
using DataLayer.Entities;
using System.Security.Cryptography;

namespace BusinessLogicLayer.Services
{
    public class AuthentificationService : IAuthService
    {
        private BankingDbContext _db;
        private readonly string _jwtSecret;
        public AuthentificationService(string jwtSecret)
        {
            _jwtSecret = jwtSecret;
        }
        public bool Authenticate(string username, string password)
        {
            Account account = _db.Accounts.FirstOrDefault(a => a.UserName == username)!;
            string hashPassword = string.Join("", SHA256.HashData(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            if (account is null)
            {
                throw new NullReferenceException("This user not exists");
            }

            return account.HashPassword == hashPassword;   
        }

        public bool Authorize(string token, string requiredRole)
        {
            var tokenHandler = new JwtSecurityTokenHandler(); // Создаем объект для работы с JWT токенами
            var key = Encoding.ASCII.GetBytes(_jwtSecret); // Преобразуем секретный ключ в байтовый массив
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Устанавливаем секретный ключ для проверки подписи токена
                    ValidateIssuer = false, // Не проверяем издателя токена
                    ValidateAudience = false, // Не проверяем аудиторию токена
                    ClockSkew = TimeSpan.Zero // Требуем точное соответствие времени токена
                };

                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var roleClaim = claimsPrincipal.FindFirst(ClaimTypes.Role); // Ищем утверждение роли в токене

                // Проверяем, соответствует ли роль указанной требуемой роли
                if (roleClaim != null && roleClaim.Value == requiredRole)
                {
                    return true; // Авторизация успешна
                }
                return false; // Недостаточно прав для авторизации
            }
            catch
            {
                return false; // Ошибка при авторизации
            }
        }

        public string GenerateToken(string userId)
        {
            /*Account currentAccount = _db.Accounts.Find(userId); // получаем аккаунт для которого будем генерить токен (?)*/
            var tokenHandler = new JwtSecurityTokenHandler(); // Создаем объект для работы с JWT токенами
            var key = Encoding.ASCII.GetBytes(_jwtSecret); 
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId), // Устанавливаем идентификатор пользователя в качестве идентификационного токена
                    /*new Claim(ClaimTypes.Role, currentAccount.Role.ToString()) // возможно спорный момент*/
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Устанавливаем срок действия токена на 7 дней
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Устанавливаем подпись токена с использованием секретного ключа
            };

            var token = tokenHandler.CreateToken(tokenDescriptor); // создаем токен

            return tokenHandler.WriteToken(token); // токен в строковом представлении
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler(); // Создаем объект для работы с JWT токенами
            var key = Encoding.ASCII.GetBytes(_jwtSecret); // Преобразуем секретный ключ в байтовый массив
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Устанавливаем секретный ключ для проверки подписи токена
                    ValidateIssuer = false, // Не проверяем издателя токена
                    ValidateAudience = false, // Не проверяем аудиторию токена
                    ClockSkew = TimeSpan.Zero // Требуем точное соответствие времени токена
                }, out SecurityToken validatedToken);
                return true; // Токен является валидным
            }
            catch
            {
                return false; // Токен недействителен
            }
        }
    }
}
