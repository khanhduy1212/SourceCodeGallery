using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using XProject.Domain.Abstract;
using XProject.Domain.Concrete;
using XProject.Domain.Entities;

namespace XProject.Web.Infrastructure.Utility
{
    public class CustomerLoginPersister
    {
        #region Repository

        private static ILoginTracker LoginTracker
        {
            get { return DependencyHelper.GetService<ILoginTracker>(); }
        }
       
        #endregion
        public static void SignIn(string email, bool persistent = false)
        {
            LoginTracker.CustomerSignIn(email, HttpContext.Current.Session.SessionID);
            FormsAuthentication.SetAuthCookie(email, persistent);
        }

        public static void SignOut()
        {
            if (HttpContext.Current != null)
            {
               LoginTracker.CustomerSignOut(HttpContext.Current.User.Identity.Name, HttpContext.Current.Session.SessionID);

                FormsAuthentication.SignOut();

               
            }
        }

        public static void SignOutForForgetPassword()
        {
            if (HttpContext.Current != null)
            {
                LoginTracker.CustomerSignOut(HttpContext.Current.User.Identity.Name);
                FormsAuthentication.SignOut();
            }
        }
           //public static BDSAccount RetrieveCustomer()
           //{
           
           //    BDSAccount account = LoginTracker.RetrieveCustomer(HttpContext.Current.Session.SessionID);
           //    // login if cookie exists
           //    if (account == null && HttpContext.Current.User.Identity.IsAuthenticated)
           //    {
           //        LoginTracker.CustomerSignIn(HttpContext.Current.User.Identity.Name, HttpContext.Current.Session.SessionID);
           //        account = LoginTracker.RetrieveCustomer(HttpContext.Current.Session.SessionID);
           //    }

           //    return account;
           //}
        //public static InMemLoginRecord RetrieveCustomerInfo()
        //{

        //    BDSAccount account = LoginTracker.RetrieveCustomer(HttpContext.Current.Session.SessionID);
        //    // login if cookie exists
        //    if (account != null && HttpContext.Current.User.Identity.IsAuthenticated)
        //    {
        //        LoginTracker.CustomerSignIn(HttpContext.Current.User.Identity.Name, HttpContext.Current.Session.SessionID);
               
        //    }
        //    return LoginTracker.RetrieveCustomerInfo(account);
           
        //}
        //public static void ReloadCustomer(BDSAccount account)
        //{


        //    LoginTracker.ReloadCustomer(account);

        //}

        public static bool IsLogged(string username)
           {
               return LoginTracker.IsCustomerLogged(username);
           }

        internal static void SignInExternal(string typeExternal, bool persistent = false)
        {
            LoginTracker.CustomerSignInExternal(typeExternal, HttpContext.Current.Session.SessionID);
            FormsAuthentication.SetAuthCookie(typeExternal, persistent);
        }
    }
}
