using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using XProject.Web.Controllers;
using FluentValidation.Mvc;
using FluentValidation.Validators;
using ServiceStack.Text;
using XProject.Web.Infrastructure;
using XProject.Web.Infrastructure.Helpers;
using XProject.Web.Infrastructure.Utility;
using NS.Mail;
using StackExchange.Profiling;
using XProject.Web.Infrastructure.ModelBinders;

namespace XProject.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapHubs();

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            FilterConfig.RegisterFilterProviders(FilterProviders.Providers);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BinderConfig.RegisterBinder(ModelBinders.Binders);
            MapperConfig.RegisterMappers();

            //FluentValidationModelValidatorProvider.Configure(x => x.Add(typeof(GreaterThanValidator), (metadata, context, rule, validator) => new GreaterThanValidatorEx(metadata, context, rule, validator)));
            FluentValidationModelValidatorProvider.Configure();
            //JsConfig.DateHandler = DateHandler.ISO8601;

            // NS.Framework configuration
            MailConfig.DefaultCssCompiler = html => PreMailer.Net.PreMailer.MoveCssInline(html, true, "#ignore").Html;
            MailConfig.DefaultRazorTemplateLocation = HttpContext.Current.Server.MapPath("~/Content/EmailTemplates");

            ////ScheduleHelper.AlertNoPayment("Subaru_AlertNoPayment", "Subaru_AlertNoPayment");
            //ScheduleHelper.BookingPaymentReminder("Subaru_BookingPaymentReminder", "Subaru_BookingPaymentReminder_1");
            //ScheduleHelper.BookingDailyReport("Subaru_BookingDailyReport", "Subaru_BookingDailyReport_1");
            //ScheduleHelper.AllocationReminder("Subaru_AllocationReminder", "Subaru_AllocationReminder");
            //ScheduleHelper.SetPriceScheduleTrigger("UpdatePriceSchedule", "UpdatePriceSchedule");
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;


            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());

            MiniProfilerEF.Initialize();
        }

        protected void Session_Start()
        {
            // http://stackoverflow.com/questions/2874078/asp-net-session-sessionid-changes-between-requests
            Session["init"] = 0;
            var ignored = MiniProfiler.Settings.IgnoredPaths.ToList();
            ignored.Add("signalr");
            MiniProfiler.Settings.IgnoredPaths = ignored.ToArray();
        }

        protected void Session_End()
        {
            
          /*
            if (CurrentUser.IsAuthenticated)
                CurrentUser.Logout();
            else if (CurrentCustomer.IsAuthenticated)
                CurrentCustomer.Logout();*/
        }

        protected void Application_BeginRequest()
        {
            try
            {
                const string sessionParamName = "ASPSESSID";
                const string sessionCookieName = "ASP.NET_SessionId";

                if (HttpContext.Current.Request.Form[sessionParamName] != null)
                    UpdateCookie(sessionCookieName, HttpContext.Current.Request.Form[sessionParamName]);
                else if (HttpContext.Current.Request.QueryString[sessionParamName] != null)
                    UpdateCookie(sessionCookieName, HttpContext.Current.Request.QueryString[sessionParamName]);

                const string authParamName = "AUTHID";
                string authCookieName = FormsAuthentication.FormsCookieName;

                if (HttpContext.Current.Request.Form[authParamName] != null)
                    UpdateCookie(authCookieName, HttpContext.Current.Request.Form[authParamName]);
                else if (HttpContext.Current.Request.QueryString[authParamName] != null)
                    UpdateCookie(authCookieName, HttpContext.Current.Request.QueryString[authParamName]);
            }
            catch
            {
            }

            //if (Request.IsLocal)
            //    MiniProfiler.Start();
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

        private void UpdateCookie(string cookieName, string cookieValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookieName) ?? new HttpCookie(cookieName);
            cookie.Value = cookieValue;
            HttpContext.Current.Request.Cookies.Set(cookie);
        }
        public static CultureInfo CultureInfo
        {
            get
            {


                //return new System.Globalization.CultureInfo((string)System.Web.HttpContext.Current.Session["CurrentCulture"]);
              return new System.Globalization.CultureInfo("en-GB"); ;
            }
        }

        public static DateTimeFormatInfo DateTimeFormat
        {
            get
            {


                return CultureInfo.DateTimeFormat;
            }
        }




        /*void Application_Error(object sender, EventArgs e)
        {
            try
            {

                var error = Server.GetLastError();
                var code = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;
                string action = null;
                switch (code)
                {
                    case 404:
                        // page not found
                        action = "Index";
                        break;
                    case 500:
                        // server error
                        action = "Index";
                        break;
                }
                var checkAdmin = HttpContext.Current.Request.Path.ToLower().Contains("/admin");
                if (!string.IsNullOrEmpty(action))
                {
                    // clear error on server
                    Response.Clear();
                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;

                    // Call target Controller and pass the routeData.
                    IController errorController = new HomeController();
                    var routeData = new RouteData();
                  
                    routeData.Values.Add("action", action);
                    if (checkAdmin)
                    {
                        routeData.Values.Add("controller", "Dashboard");
                        routeData.DataTokens.Add("area", "Admin");
                    }
                    else
                    {
                        routeData.Values.Add("controller", "Home");
                    }
                    errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                }
            }
            catch (Exception)
            {


            }



        }*/
    }
}
