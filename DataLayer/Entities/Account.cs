using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Account
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string HashPassword { get; set; } = null!;
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public Client Client { get; set; } = null!;
        public Administrator Administrator { get; set; } = null!;
        public Manager Manager { get; set; } = null!;
        public List<BankAccount> BankAccounts { get; set; } = null!; // сделать во флюенте связь один ко многим
    }
}
