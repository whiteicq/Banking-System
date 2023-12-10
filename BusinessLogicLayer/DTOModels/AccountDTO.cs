using BusinessLogicLayer.DTOModels;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOModels
{
    public class AccountDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string HashPassword { get; set; } = null!;
        public Roles Role { get; set; }
        public List<BankAccountDTO> BankAccounts { get; set; } = null!;
    }
}
