using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string HashPassword { get; set; } = null!;
        [Column(TypeName = "varchar(20)")]
        public Roles Role { get; set; }
        public DateTime DateBirth { get; set; }
        public List<BankAccount>? BankAccounts { get; set; } = null!;
    }
}
