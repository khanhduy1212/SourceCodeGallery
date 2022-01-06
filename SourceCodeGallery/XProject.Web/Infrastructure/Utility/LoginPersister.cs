using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;
using System.Diagnostics;

namespace XProject.Web.Infrastructure.Utility
{
    public class LoginPersister
    {
        #region Repository

        private static ILoginTracker LoginTracker
        {
            get { return DependencyHelper.GetService<ILoginTracker>(); }
        }

        #endregion
        public static void SignIn(string email, bool persistent = false)
        {
            LoginTracker.SignIn(email, HttpContext.Current.Session.SessionID);
            //Log.Debug(LoginTracker.IsLogged("admin@localhost"),"is login");

            FormsAuthenticationTicket _ticket = new FormsAuthenticationTicket(1,email,DateTime.Now,DateTime.MaxValue, persistent,email);
            
            string _encryptedTicket = FormsAuthentication.Encrypt(_ticket);

            HttpCookie _cookie = new HttpCookie("AdminAuthentication", _encryptedTicket);

            HttpContext.Current.Response.Cookies.Add(_cookie);


          
        }

        public static void SignOut()
        {
            if (HttpContext.Current != null)
            {

                HttpCookie _cookie = HttpContext.Current.Request.Cookies["AdminAuthentication"];
                if (_cookie != null)
                {

                    string _encryptedTicket = _cookie.Value;
                    FormsAuthenticationTicket _ticket = FormsAuthentication.Decrypt(_encryptedTicket);
                  
                    IIdentity _identity = new FormsIdentity(_ticket);                    
                    LoginTracker.SignOut(_identity.Name, HttpContext.Current.Session.SessionID);
                  
                }
                else
                {
                    LoginTracker.SignOut("", HttpContext.Current.Session.SessionID);

                }
              
                HttpCookie cookie = new HttpCookie("AdminAuthentication")
                {
                    Expires = DateTime.Now.AddDays(-1) // or any other time in the past
                };
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }

        public static UserLogin RetrieveUser()
        {
         
            if (HttpContext.Current == null)
            {
                //Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":1.RetrieveUser:HttpContext.Current == null");
                return null;
            }
            //Trace.WriteLine( HttpContext.Current.Session.SessionID + ":1.RetrieveUser:HttpContext.Current.Session:"+ HttpContext.Current.Session.SessionID);
            UserLogin userLogin = LoginTracker.RetrieveUser(HttpContext.Current.Session.SessionID);
           // Trace.WriteLine(HttpContext.Current.Session.SessionID + ":2.RetrieveUser:HttpContext.Current.Session:" + userLogin);
            //Log.Debug("Debug 2");
            // login if cookie exists

            try
            {
                HttpCookie _cookie = HttpContext.Current.Request.Cookies["AdminAuthentication"];
                //Trace.WriteLine(HttpContext.Current.Session.SessionID+ ":3.RetrieveUser:Cookies['AdminAuthentication']:" + (_cookie == null ? "NULL" : "Expires:" + _cookie.Expires.ToString("dd/MM/yyyy HH:mm:ss")));
                if (_cookie != null)
                {

                    string _encryptedTicket = _cookie.Value;
                    //Trace.WriteLine( HttpContext.Current.Session.SessionID+ ":4.RetrieveUser:Cookies['AdminAuthentication']:" + (_cookie == null ? "NULL" : "Data:" + _cookie.Value));
                    FormsAuthenticationTicket _ticket = FormsAuthentication.Decrypt(_encryptedTicket);
                   // Trace.WriteLine( HttpContext.Current.Session.SessionID + ":5.FormsAuthenticationTicket:Cookies['AdminAuthentication']:" + (_ticket == null ? "NULL" : "Expired:" + _ticket.Expired));
                    if (!_ticket.Expired)
                    {
                        IIdentity _identity = new FormsIdentity(_ticket);
                       // Trace.WriteLine( HttpContext.Current.Session.SessionID + ":6.FormsAuthenticationTicket:FormsIdentity:" + (_identity == null ? "NULL" : "Name:" + _identity.Name));
                        IPrincipal _principal = new GenericPrincipal(_identity, new string[0]); //Identity plus string of roles.
                       // Trace.WriteLine( HttpContext.Current.Session.SessionID + ":7.FormsAuthenticationTicket:IPrincipal:" + (_principal == null ? "NULL" : "Name:" + _principal.Identity.Name));
                       // Trace.WriteLine(HttpContext.Current.Session.SessionID + ":7.1.FormsAuthenticationTicket:IPrincipal:" + (_principal == null ? "NULL" : "IsAuthenticated:" + _principal.Identity.IsAuthenticated));
                        if (userLogin == null && _principal.Identity.IsAuthenticated)
                        {
                          //  Trace.WriteLine(HttpContext.Current.Session.SessionID+ ":8.SignIn:" + _principal.Identity.Name+" - "+ HttpContext.Current.Session.SessionID);
                            LoginTracker.SignIn(_principal.Identity.Name, HttpContext.Current.Session.SessionID);
                            //Log.Debug("Login by email");
                            userLogin = LoginTracker.RetrieveUser(HttpContext.Current.Session.SessionID);
                            //Log.Debug("Retrieve UserLogin");
                        }
                    }
                }
            }
            catch (Exception e)
            {

                Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + " - " + HttpContext.Current.Session.SessionID) + ":9.Exception:" +e.Message+"\r\n"+ e.StackTrace);
            }
           


            return userLogin;
        }

        public static bool IsLogged(string username)
        {
            return LoginTracker.IsLogged(username);
        }
    }
}
