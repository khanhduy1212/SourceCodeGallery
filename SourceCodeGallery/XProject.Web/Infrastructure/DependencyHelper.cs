using System.Web.Mvc;

namespace XProject.Web.Infrastructure
{
    public class DependencyHelper
    {
        public static T GetService<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }
    }
}
