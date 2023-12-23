using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime DateTransaction { get; set; }
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; } = null!;
        public decimal SumTransaction { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "varchar(20)")]
        public TransactionType TransactionType { get; set; }
    }
}
