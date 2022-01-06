using System.Diagnostics;
using System.Reflection;
using System.Web.Mvc;
using NS.Mvc.ModelBinders;
using XProject.Domain;

namespace XProject.Web
{
    public static class BinderConfig
    {
        public static void RegisterBinder(ModelBinderDictionary binders)
        {
            EnumerationBinderHelper.RegisterBinders(binders, typeof (Repository).Assembly);
            EnumerationBinderHelper.RegisterBinders(binders, typeof(BinderConfig).Assembly);
        }
    }
}
