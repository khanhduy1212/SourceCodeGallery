using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace XProject.Web.Infrastructure.Filters
{
    public class ExcludeFiltersAttribute : FilterAttribute
    {
        public ExcludeFiltersAttribute(params Type[] types)
        {
            if (Filters == null)
                Filters = new List<Type>();

            Filters.AddRange(types);
        }

        public List<Type> Filters { get; private set; }
    }
}
