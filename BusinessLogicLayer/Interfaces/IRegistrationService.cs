using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.DTOModels;
using DataLayer.EF;
using DataLayer.Entities;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRegistrationService
    {
        Account CreateAccount(string nickname, string email, string password, string phoneNumber, DateTime dateBirth);
        
    }   
}
