using System.Configuration;

namespace XProject.Web.Infrastructure
{
    public class TokenLogin
    {
        public static string Generate()
        {
            return "";
        }

        public static bool ValidateToken(string token)
        {
            if (token == ConfigurationManager.AppSettings["token"])
                return true;
            return false;
        }
    }
}
