using System;
using System.Collections.Generic;
using XProject.Domain.Entities;
using XProject.Domain.Enum;

namespace XProject.Domain.Abstract
{
    public interface IMembershipService : IAuthenticationService
    {
        IEnumerable<UserLogin> GetUsers(Func<UserLogin, bool> predicate);
        IEnumerable<UserLogin> GetUsers();
        IEnumerable<UserLogin> GetUsersDeleted();
        UserLogin GetUser(int id);
        UserLogin GetUserByName(string username);
        UserLogin GetUserByEmail(string email);
        UserLogin CreateUser(UserLogin userLoginInfo);
        bool UpdateUser(UserLogin userLoginInfo);
        void UpdateUserLanguage(int id, int languageID);
        void UpdateUserPicture(int id, string fileName);
        bool DeleteUser(int id);
        IEnumerable<UserLogin> GetUsersInRole(params string[] roleName);
        IEnumerable<UserLogin> GetUsersContainsInRole(params string[] roleName);

        //Dealer CreateDealer(Dealer UserLogin, int countryID);
        //Dealer GetDealer(string dealercode);
        //Dealer GetDealer(int id);
        //bool UpdateDealer(Dealer item);
        bool ReActiveUser(int id);
        IEnumerable<UserLogin> GetAllUserByEmail(string email);
        IEnumerable<UserLogin> GetAllUser();
        IEnumerable<UserLogin> GetAllUser(Func<UserLogin, bool> predicate);


    }
    public interface IMembershipServiceBase
    {
        IEnumerable<UserLogin> GetAllUsers(Func<UserLogin, bool> predicate);
    }
}
