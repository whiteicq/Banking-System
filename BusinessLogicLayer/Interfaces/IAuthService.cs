using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(string userId);
        bool ValidateToken(string token);
        bool Authenticate(string username, string password);
        bool Authorize(string token, string requiredRole);
    }
}
