using XProject.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace XProject.Web.Areas.Admin.Helper
{
    public class PropertyInfoHelper
    {
       
        public static Func<T, T> DynamicSelectGenerator<T>(string Fields = "")
        {
            string[] EntityFields;
            //if (Fields == "")
            //    // get Properties of the T
            //    EntityFields = typeof(T).GetProperties().Select(propertyInfo => propertyInfo.Name).ToArray();
            //else
            EntityFields = Fields.Split(';').Where(x => x != "").ToArray();

            // input parameter "o"
            var xParameter = Expression.Parameter(typeof(T), "o");

            // new statement "new Data()"
            var xNew = Expression.New(typeof(T));
            // create initializers
            var bindings = EntityFields.Select(o => o.Trim())
                .Select(o =>
                {
                    // property "Field1"
                    var mi = typeof(T).GetProperty(o);
                    // original value "o.Field1"
                    var xOriginal = Expression.Property(xParameter, mi);
                    // set value "Field1 = o.Field1"
                    var result =  Expression.Bind(mi, xOriginal);
                    return result;
                }
            );
            // initialization "new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var xInit = Expression.MemberInit(xNew, bindings);
            // expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<T, T>>(xInit, xParameter);
            // compile to Func<Data, Data>
            return lambda.Compile();
        }

      

    }
}