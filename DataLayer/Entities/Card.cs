using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Card
    {
        public int Id { get; set; }
        [MaxLength(16)]
        public string CardNumber { get; set; } = null!;
        [MaxLength(3)]
        public string Cvv { get; set; } = null!;
        public DateTime DateExpiration { get; set; }
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; } = null!;
    }
}
