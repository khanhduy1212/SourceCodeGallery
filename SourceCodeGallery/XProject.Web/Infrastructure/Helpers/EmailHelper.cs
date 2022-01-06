using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Hosting;
using ColorCode.Compilation.Languages;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;
using XProject.Domain.Enum;
using NS.Helpers;
using NS.Mail;
using Resources;

using System.Net.Configuration;
using System.Text;
using System.Web;
using XProject.Web.Properties;

namespace XProject.Web.Infrastructure.Helpers
{
    public class EmailHelper
    {
        public static string RenderMessage(string path, object model = null)
        {
            try
            {
                var mail = new RazorMailMessage(path, model).Render();
                var stream = mail.AlternateViews[0].ContentStream;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                //var a = new ExternalException("template file:" + path + "\n\n" + ex.Message);
                //throw a;
                return "";
            }
        }
        public static void SendEmail(string[] to, string subject, RazorMailMessage message)
        {
            Mailer.UseMessage(message)
                .From("East West <no-reply@XProject.com>")
                .To(to)
                .Subject(subject)
                .SendAsync();
        }
        //public static void SendEmail(string[] to, string[] cc, string[] bcc, string subject, RazorMailMessage message)
        //{
        //    Mailer.UseMessage(message)
        //          .From(FontendResource.duythanharchitecture + " <no-reply@XProject.com>")
        //          .To(to)
        //          .CC(cc)
        //          .BCC(bcc)
        //          .Subject(subject)
        //          .SendAsync();
        //}
        private static void SendEmailAddBcc(IEnumerable<string> to, string subject, RazorMailMessage message, bool bcc = true)
        {
            var mailFrom = new MailAddress("no-reply@XProject.com", "East West");
            var mail = new MailMessage { Subject = subject, From = mailFrom };
            foreach (var s in to)
            {
                mail.To.Add(s);
            }
            //#region additional signature image
            //var path = HostingEnvironment.MapPath(@"~/Content/images/signature.jpg");
            //if (path != null)
            //{
            //    var yourPictureRes = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
            //    yourPictureRes.ContentId = "signature";
            //    message.AlternateViews[0].LinkedResources.Add(yourPictureRes);
            //}
            //#endregion additional signature image

            mail.AlternateViews.Add(message.AlternateViews[0]);
            var mySmtpClient = new SmtpClient();
            if (bcc)
            {
                var bccReceivers = ConfigurationManager.AppSettings["EmailBcc"];
                if (bccReceivers != null && !string.IsNullOrEmpty(bccReceivers))
                {
                    var bccemails = bccReceivers.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var bccemail in bccemails)
                    {
                        mail.Bcc.Add(bccemail);
                    }
                }
            }
            mySmtpClient.Send(mail);

        }
        private static void SendEmail(IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, string subject, RazorMailMessage message)
        {
            var mailFrom = new MailAddress("no-reply@XProject.com", "East West");
            var mail = new MailMessage { Subject = subject, From = mailFrom };
            foreach (var s in to)
            {
                mail.To.Add(s);
            }
            foreach (var s in cc)
            {
                mail.CC.Add(s);
            }
            foreach (var s in bcc)
            {
                mail.Bcc.Add(s);
            }
            mail.AlternateViews.Add(message.AlternateViews[0]);
            var mySmtpClient = new SmtpClient();
            mySmtpClient.Send(mail);

        }

        public static bool SendEmail(string from, string to, string subject, string body, IEnumerable<string> cc, IEnumerable<string> bcc, List<string> attachmentFilename)
        {
           
            try
            {
                var section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                var message = new MailMessage(from, to, subject, body) { IsBodyHtml = true };
                message.From = new MailAddress(section.Network.UserName, "East West Planners Pte Ltd", Encoding.UTF8);
                var client = new SmtpClient();
                client.Port = section.Network.Port;
                client.Host = section.Network.Host;
                client.EnableSsl = section.Network.EnableSsl;//true;
                // setup up the host, increase the timeout to 60 minutes
                client.Timeout = (60 * 60 * 1000);
                client.DeliveryMethod = section.DeliveryMethod;//SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(section.Network.UserName, section.Network.Password);
                //Attatch file
                for (int i = 0; i < attachmentFilename.Count; i++)
                {
                    var fullPath = HttpContext.Current.Server.MapPath("~/data/attachments/"+ attachmentFilename[i]);
                    var attachment = new System.Net.Mail.Attachment(fullPath, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(fullPath);
                    disposition.ModificationDate = File.GetLastWriteTime(fullPath);
                    disposition.ReadDate = File.GetLastAccessTime(fullPath);
                    disposition.FileName = Path.GetFileName(fullPath);
                    disposition.Size = new FileInfo(fullPath).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    message.Attachments.Add(attachment);
                }

                foreach (var s in cc)
                {
                    message.CC.Add(s);
                }
                foreach (var s in bcc)
                {
                    message.Bcc.Add(s);
                }
                client.Send(message);

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public static void SubscribeGiftEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var subject = FontendResource.email_subject_subscribe_gift;
                var message = new RazorMailMessage("Customer/SubscribeGift").Render();
                SendEmailAddBcc(new[] { email }, subject, message);
            }
        }
        /*
        public static void NewCustomer(Customer customer)
        {
            if (customer != null && !string.IsNullOrEmpty(customer.Email))
            {
                var subject = FontendResource.email_subject_newcustomer;
                var message = new RazorMailMessage("Customer/NewCustomer", customer).Render();
                SendEmailAddBcc(new[] { customer.Email }, subject, message, false);
            }
        }

        public static void ForgetPassword(Contact contact)
        {
            if (contact != null && !string.IsNullOrEmpty(contact.Email))
            {
                var subject = "Reset your password";
                var message = new RazorMailMessage("Contact/ForgetPassword", contact).Render();
                SendEmailAddBcc(new[] { contact.Email }, subject, message, false);
            }
        }
        public static void ResetPassword(Contact contact)
        {
            if (contact != null && !string.IsNullOrEmpty(contact.Email))
            {
                var subject = "Reset password";
                var message = new RazorMailMessage("Contact/ResetPassword", contact).Render();
                SendEmailAddBcc(new[] { contact.Email }, subject, message, false);
            }
        }
        public static void AgentRegistrationSuccess(Contact contact)
        {
            if (contact != null && !string.IsNullOrEmpty(contact.Email))
            {
                var subject = "Reset password";
                var message = new RazorMailMessage("Contact/AgentRegistrationSuccess", contact).Render();
                SendEmailAddBcc(new[] { contact.Email }, subject, message, false);
            }
        }
        public static void ServiceEnquiry(DirectoryInfo directoryInfo, Case item)
        {
            var unitRepo = DependencyHelper.GetService<IUnitRepository>();
            var emailSetting = unitRepo.GetEmailSetting(directoryInfo.Name, "Enquiry");
            if (emailSetting != null)
            {
                var to = !string.IsNullOrEmpty(emailSetting.EmailTo)
                    ? emailSetting.EmailTo.Split(new[] {',', ';'})
                    : new string[] {};
                var cc = !string.IsNullOrEmpty(emailSetting.EmailCc)
                    ? emailSetting.EmailCc.Split(new[] {',', ';'})
                    : new string[] {};
                var bcc = !string.IsNullOrEmpty(emailSetting.EmailBcc)
                    ? emailSetting.EmailBcc.Split(new[] {',', ';'})
                    : new string[] {};
                if (item != null && to.Length > 0)
                {
                    var subject = item.Subject + " Enquiry";
                    var message = new RazorMailMessage("Customer/Enquiry", item).Render();
                    SendEmail(to, cc, bcc, subject, message);
                }
            }
        }

        public static void ContactUs(Case Case)
        {
            if (Case != null)
            {
                var subject = "Enquiry";
                var message = new RazorMailMessage("Customer/Enquiry", Case).Render();
                SendEmailAddBcc(new[] { Case.EmailAddress }, subject, message, false);
            }
        }

        public static void SubmittedItinerary(SendEmailSubmittedItineraryModel model)
        {
            if (model != null)
            {
                var subject = "Itinerary " + model.ItineraryName + " submitted for Approval";
                var message = new RazorMailMessage("Itinerary/Submitted", model).Render();
                SendEmailAddBcc(new[] { model.Email }, subject, message, false);
            }
        }*/


    }
}