using System;
using System.Web.Mvc;
using XProject.Domain.Helpers.Encryption;
using UrlHelper = XProject.Web.Infrastructure.Helpers.UrlHelper;

namespace XProject.Web.Infrastructure.Utility
{
    public static class BypassAuth
    {
        private static IStringEncryptor Encryptor
        {
            get { return DependencyResolver.Current.GetService<IStringEncryptor>(); }
        }

        public static string Encrypt(string url, string username)
        {
            string data = url + "::" + username;
            string token = Encryptor.Encrypt(data);

            return UrlHelper.Absolute("/m/" + token);
        }

        public static string Encrypt(string url)
        {
            string data = url;
            string token = Encryptor.Encrypt(data);

            return UrlHelper.Absolute("/n/" + token);
        }
        public static string Decrypt(string token)
        {
            string data = Encryptor.Decrypt(token);
            string[] segments = data.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
            string url = segments[0];
            if (segments.Length > 1 && !string.IsNullOrEmpty(segments[1]))
            {
                //string username = segments[1];
                //LoginPersister.SignIn(username);
            }
            return url;
        }
    }
}
