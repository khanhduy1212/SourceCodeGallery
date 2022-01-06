using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;
using XProject.Domain.Helpers;
using XProject.Web.Infrastructure.Helpers;
using XProject.Web.Infrastructure.Utility;
using Newtonsoft.Json;
using NS.Mail;

namespace XProject.Web.Infrastructure.Filters
{
    public class ErrorLoggerAttribute : HandleErrorAttribute
    {
        private static IAuditTracker AuditTracker
        {
            get { return DependencyHelper.GetService<IAuditTracker>(); }
        }
        public override void OnException(ExceptionContext filterContext)
        {
            LogError(filterContext);
            base.OnException(filterContext);
        }

        private void LogError(ExceptionContext filterContext)
        {
            // You could use any logging approach here
            if (HttpContext.Current.Request.IsAjaxRequest() ||
                filterContext.Exception == null ||
                filterContext.Exception.Message.Contains("The remote host closed the connection"))
                return;
            string username = "";
            string email = "";
            if (CurrentUser.Identity != null)
            {
                username = CurrentUser.Identity.DisplayName;
                email = CurrentUser.Identity.Email;
            }
            HttpRequest request = HttpContext.Current.Request;
            var record = new Audit
            {
                Username = username,
                Email = email,
                IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                UrlAccessed = request.RawUrl,
                TimeAccessed = DateTime.Now,
                SessionId = HttpContext.Current.Session.SessionID,
                Message = "Log Exception",
                Type = (int)AuditType.Exception,
                ActionId = 0
            };
            try
            {
                record.Data = JsonConvert.SerializeObject(new
                {
                    filterContext.Exception.InnerException,
                    filterContext.Exception.Source,
                    filterContext.Exception.TargetSite,
                    filterContext.Exception.Message,
                    filterContext.Exception.Data,
                    filterContext.Exception.StackTrace
                });
            }
            catch (Exception)
            {
            }
            AuditTracker.CreateRecord(record);
            LogsSendEmail(record);
        }
        private void LogsSendEmail(Audit audit)
        {/*
            string subject = ConfigurationManager.AppSettings["SystemName"] + " System Logs Exception " + HttpContext.Current.Request.Url.Host;
            var message = new RazorMailMessage("Logs/Exception", audit).Render();
            var receiveEmail = ConfigurationManager.AppSettings["LogsEmail"];
            if (receiveEmail != null && !string.IsNullOrEmpty(receiveEmail))
            {
                var emails = receiveEmail.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                EmailHelper.SendEmail(emails, subject, message);
            }*/
        }
    }
}