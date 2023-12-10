using BusinessLogicLayer.Interfaces;
using DataLayer.EF;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BusinessLogicLayer.Services
{
    public class ManagerRegistrationService : IRegistrationService
    {
        BankingDbContext _db;

        public ManagerRegistrationService(BankingDbContext db)
        {
            _db = db;
        }

        public void CreateAccount(string nickname, string email, string password, string phoneNumber, DateTime dateBirth)
        {
            Account newAccount = new Account()
            {
                UserName = nickname,
                Email = email,
                HashPassword = string.Join("", SHA256.HashData(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2"))),
                PhoneNumber = phoneNumber,
                Role = DataLayer.Roles.Manager,
                DateBirth = dateBirth
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
