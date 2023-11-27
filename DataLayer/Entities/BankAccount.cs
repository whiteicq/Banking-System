using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class BankAccount
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(20)")]
        public BankAccountType AccountType { get; set; }
        public decimal Balance { get; set; }
        public DateTime DateCreate { get; set; }
        public Account Account { get; set; } = null!;
        public int AccountId { get; set; }
        public bool IsFrozen { get; set; } = false;
        public List<Card> Cards { get; set; } = null!;
        public List<Credit> Credits { get; set; } = null!;
        public List<Transaction> Transactions { get; set; } = null!;
    }
}
