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
using Microsoft.Extensions.Configuration;

namespace BusinessLogicLayer.Services
{
    public class AuthentificationService : IAuthService
    {
        private BankingDbContext _db;
        private IEmailService _emailService;

        public AuthentificationService(BankingDbContext db)
        {
            _db = db;
            _emailService = new EmailService();
        }
        public bool Authentificate(string username, string password)
        {
            Account account = _db.Accounts.FirstOrDefault(a => a.UserName == username)!;
            string hashPassword = string.Join("", SHA256.HashData(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            if (account is null)
            {
                throw new NullReferenceException("This user not exists");
            }

            return (account.UserName == username) && (account.HashPassword == hashPassword);
        }

        private string GenerateAuthCode()
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                int r = random.Next(0, 10);
                sb.Append(r);
            }

            return sb.ToString();
        }
    }
}
