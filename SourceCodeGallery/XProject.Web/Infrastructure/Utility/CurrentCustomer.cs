using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime.Misc;
using XProject.Domain.Abstract;
using XProject.Domain.Concrete;
using XProject.Domain.Entities;
using XProject.Domain.Enum;
using XProject.Domain.Helpers;
using StackExchange.Profiling.Helpers.Dapper;

namespace XProject.Web.Infrastructure.Utility
{
    public class CurrentCustomer
    {
        #region Repository

        private static IAuthenticationService AuthenticationService
        {
            get { return DependencyResolver.Current.GetService<IAuthenticationService>(); }
        }

        private static IAuthorizationService AuthorizationService
        {
            get { return DependencyResolver.Current.GetService<IAuthorizationService>(); }
        }

        private static IUnitRepository UnitRepo
        {
            get { return DependencyHelper.GetService<IUnitRepository>(); }
        }

        //private static IGeneralRepository<BDSAccount> AccountService
        //{
        //    get { return DependencyResolver.Current.GetService<IGeneralRepository<BDSAccount>>(); }
        //} 
        #endregion

        //public static bool IsAuthenticated
        //{
        //    get
        //    {
        //        return Identity != null;
        //    }
        //}
        
        //public static BDSAccount Identity
        //{
        //    get { return CustomerLoginPersister.RetrieveCustomer(); }
        //}
        //public static InMemLoginRecord IdentityInfo
        //{
        //    get { return CustomerLoginPersister.RetrieveCustomerInfo(); }
        //}
        public static bool Login(string email, string password, bool rememberMe = false)
        {
            //if (string.IsNullOrEmpty(email)) return false;
            //var endcodePassword = EncryptHelper.EncryptPassword(password);
            //var user = AccountService.GetIQueryableItems().FirstOrDefault(m => 
            //                                                 m.Email.ToLower() == email.ToLower() &&
            //                                                 m.PassWord == endcodePassword);
            //if (user != null)
            //{
            //    CustomerLoginPersister.SignIn(user.Email, rememberMe);
            //    return Identity != null;
            //}
            return false;
        }

        public static void Logout()
        {
            CustomerLoginPersister.SignOut();
        }
     
    }
}
