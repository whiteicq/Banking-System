using BusinessLogicLayer.DTOModels;
using BusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EF;
using DataLayer.Entities;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace BusinessLogicLayer.Services
{
    public class ClientRegistrationService : IRegistrationService
    {
        private BankingDbContext _db;

        public ClientRegistrationService(BankingDbContext db)
        {
            _db = db;
        }

        public void CreateAccount(string nickname, string email, string password, string phoneNumber, DateTime dateBirth)
        {
            // уникальность ника, почты и телефона гарантируются на уровне бд (???)
            if (IsAccountExistsByEmail(email))
            {
                throw new InvalidOperationException("Account with this email already exists");
            }
            if (IsAccountExistsByUsername(nickname))
            {
                throw new InvalidOperationException("Account with this user name already exists");
            }

            Account newAccount = new Account()
            {
                UserName = nickname,
                Email = email,
                HashPassword = string.Join("", SHA256.HashData(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2"))),
                PhoneNumber = phoneNumber,
                Role = DataLayer.Roles.Client,
                DateBirth = dateBirth,
                BankAccounts = new List<DataLayer.BankAccount>()
            };

            _db.Accounts.Add(newAccount);
            _db.SaveChanges();
        }

        private bool IsAccountExistsByEmail(string email)
        {
            var accounts = _db.Accounts.Where(a => a.Email == email);
            return accounts.Any();
        }

        private bool IsAccountExistsByUsername(string userName)
        {
            var accounts = _db.Accounts.Where(a => a.UserName == userName);
            return accounts.Any();
        }
    }
}
