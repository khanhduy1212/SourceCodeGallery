using System.Collections.Generic;
using XProject.Domain.Entities;

namespace XProject.Domain.Abstract
{
    public interface IAuthenticationService
    {
        UserLogin ValidateLogin(string email, string password);
    
    }
}