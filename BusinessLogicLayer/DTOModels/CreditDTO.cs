using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOModels
{
    public class CreditDTO
    {
        public int Id { get; set; }
        public decimal SumCredit { get; set; }
        public float InterestRate { get; set; }
        public int BankAccountId { get; set; }
    }
}
