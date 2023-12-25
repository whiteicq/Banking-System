using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [MaxLength(28), MinLength(28)]
        public string IBAN { get; set; } = null!;
        public decimal Balance { get; set; }
        public DateTime DateCreate { get; set; }
        public Account Account { get; set; } = null!;
        public int AccountId { get; set; }
        public bool IsFrozen { get; set; } = false;
        [Column(TypeName = "varchar(20)")]
        public Currency Currency { get; set; }
        public  List<Card> Cards { get; set; } = new List<Card>();
        public List<Credit> Credits { get; set; } = new List<Credit>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
