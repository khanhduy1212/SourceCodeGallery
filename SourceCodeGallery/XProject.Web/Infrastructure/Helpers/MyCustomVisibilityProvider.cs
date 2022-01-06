using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XProject.Web.Infrastructure.Helpers
{
    public class MyCustomVisibilityProvider : ISiteMapNodeVisibilityProvider
    {
        public bool AppliesTo(string providerName)
        {
            return true;
        }

        public bool IsVisible(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            if (node == null)
            {
                return true;
            }

            // Is a visibility attribute specified?
            if (node.Attributes.ContainsKey("visibility"))
            {
                string visibility = node.Attributes["visibility"].ToString();
                if (string.IsNullOrEmpty(visibility))
                {
                    return true;
                }
                if (visibility == "true")
                {
                    return true;
                }
                return false;
            }



            return true;
        }

        
    }
}