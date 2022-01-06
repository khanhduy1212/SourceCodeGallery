using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Elasticsearch.Net.ConnectionPool;
using Nest;
using NS;
using ServiceStack;

namespace XProject.Domain
{
    
    public partial class NestSearch
    {
        public  readonly string Project = "EastWestSearch";
        private List<PropertyInfo> _eastWestDbSetProperties;
        private DbContext _dbContext;
        private List<Type> _eastWestDbType;
        private Dictionary<String, List<Object>> listData = new Dictionary<String, List<object>>();
        private List<Type> _eastWestDbTypeLoad;
        private List<PropertyInfo> _eastWestDbSetPropertiesLoad;
        public NestSearch(DbContext context)
        {
           
            _eastWestDbSetProperties = new List<PropertyInfo>();
            _eastWestDbType = new List<Type>();
            _eastWestDbSetPropertiesLoad = new List<PropertyInfo>();
            _eastWestDbTypeLoad = new List<Type>();
            _dbContext = context;
            var properties = context.GetType().GetProperties();
            foreach (var property in properties)
            {
                var setType = property.PropertyType;
                var isDbSet = setType.IsGenericType && (typeof(IDbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()) || setType.GetInterface(typeof(IDbSet<>).FullName) != null);
                if (isDbSet)
                {
                  //  if (setType.GetInterface(typeof(IDbSet<>).FullName).GenericTypeArguments()[0].Name.ToLower().IndexOf("quote") > -1 || setType.GetInterface(typeof(IDbSet<>).FullName).GenericTypeArguments()[0].Name.ToLower().IndexOf("invoice") > -1)
                    {
                        _eastWestDbType.Add(setType.GetInterface(typeof(IDbSet<>).FullName).GenericTypeArguments()[0]);
                        _eastWestDbSetProperties.Add(property);
                        Debug.WriteLine(setType.GetInterface(typeof(IDbSet<>).FullName).GenericTypeArguments()[0].Name.ToLower());
                      
                    }
                    _eastWestDbSetPropertiesLoad.Add(property);
                    _eastWestDbTypeLoad.Add(setType.GetInterface(typeof(IDbSet<>).FullName).GenericTypeArguments()[0]);

                 
                    
                }
              
            }
           
          
        }



        public bool InsertData(object entity)
        {
             try
            {
                var dataETD = ConvertToExpandoForNest(entity);
                return  InsertData(dataETD);
            }
            catch (Exception)
            {
                            
            }
            return false;
        }

        public  bool InsertData(ExpandoObject expandoObject)
        {

            bool status=false;
       
             ElasticClient client = ConnectionToES.EsClient();
            if (ConnectionToES.EsClient().IndexExists((expandoObject as IDictionary<string, object>)["ClassForName"].ToString()).Exists)
            {
               
                String jsonPost = "{\"index\": {\"blocks\": {\"read_only_allow_delete\": \"false\"}}}";
                List<String> listStr = ConnectionToES.PostJsonDataServer((expandoObject as IDictionary<string, object>)["ClassForName"].ToString() + @"/_settings", jsonPost);
                foreach (var str in listStr)
                {
                    Debug.WriteLine("PUT========>" + str);
                }
            }
      
            var response = client.Index(expandoObject, i => i
                .Index((expandoObject as IDictionary<string, object>)["ClassForName"].ToString())
                .Type("EastWestSearch")
                .Id((expandoObject as IDictionary<string, object>)["ID"].ToString())
                .Refresh(true));
            if (response.IsValid)
            {

                status = true;
            }
          
            Debug.WriteLine("insertData:" + (expandoObject as IDictionary<string, object>)["ClassForName"].ToString() + " - " + (expandoObject as IDictionary<string, object>)["ID"].ToString() + " is " + (status ? "success" : "error"));
            return status;
        }

       
        private  ExpandoObject ConvertToExpandoForNest(object obj, int level = 0)
        {
            if (level > 1 || obj==null)
            {
                return null;
            }
            var props = obj.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute))
                ).Select(prop => new { p = prop, pName = prop.Name, a = Attribute.GetCustomAttribute(prop, typeof(System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute)), aName = ((System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute)Attribute.GetCustomAttribute(prop, typeof(System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute))).Name });

            var propsNotMap = obj.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute))
                ).Select(prop => new { p = prop, pName = prop.Name, a = Attribute.GetCustomAttribute(prop, typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute)) });


            //Get Properties Using Reflections
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] properties = obj.GetType().GetProperties(flags);
       
            ExpandoObject expando = new ExpandoObject();
           
            foreach (PropertyInfo property in properties)
            {

                if (props.Where(T => T.aName == property.Name).ToList().Count == 0 && props.Where(T => T.pName == property.Name).ToList().Count == 0  && propsNotMap.Where(T => T.pName == property.Name).ToList().Count == 0)
                {
                    if (!(property.PropertyType.GenericTypeArguments().Length > 0 && property.PropertyType.GetGenericTypeDefinition().Namespace == "System.Collections.Generic"))
                    {
                        if (!property.GetGetMethod().IsVirtual)
                        {
                            if (!property.PropertyType.IsSubclassOf(typeof(Enumeration)))
                            {
                                
                                    if (level==1)
                                    {

                                        if (!property.Name.ToLower().Contains("password") && property.PropertyType.Namespace.IndexOf("XProject.Domain.Entities")==-1)
                                        {
                                            
                                            try
                                            {
                                                object oData = property.GetValue(obj, null);

                                                string nameType = property.PropertyType.ToString();

                                                string[] nameTypesAccept = new string[] { "long", "datetime", "int", "float", "double", "decimal", "boolean" ,"enum"};
                                                if (nameTypesAccept.Where(t => nameType.ToLower().Contains(t)).Count() > 0)
                                                {
                                                    if (oData != null)
                                                    {

                                                        AddProperty(expando, property.Name + "_string", oData.ToStringCS());
                                                    }
                                                    else
                                                    {
                                                        AddProperty(expando, property.Name + "_string", "");
                                                    }
                                                }
                                               
                                                AddProperty(expando, property.Name, oData);
                                            }
                                            catch (Exception e)
                                            {
                                                Debug.WriteLine("====>Error:"+e.Message);
                                                AddProperty(expando, property.Name, null);
                                            }
                                           
                                        }
                                    }
                                    else
                                    {
                                        if (property.PropertyType.Namespace.IndexOf("XProject.Domain.Entities")==-1)
                                        {
                                            try
                                            {
                                                object oData = property.GetValue(obj, null);

                                                string nameType = property.PropertyType.ToString();

                                                string[] nameTypesAccept = new string[] { "long", "datetime", "int", "float", "double", "decimal", "boolean", "enum" };
                                                if (nameTypesAccept.Where(t => nameType.ToLower().Contains(t)).Count() > 0)
                                                {
                                                    if (oData != null)
                                                    {
                                                        AddProperty(expando, property.Name + "_string", oData.ToStringCS());
                                                    }
                                                    else
                                                    {
                                                        AddProperty(expando, property.Name + "_string", null);
                                                    }
                                                }
                                                AddProperty(expando, property.Name, oData);
                                            }
                                            catch (Exception e)
                                            {
                                                Debug.WriteLine("====>Error:" + e.Message);
                                                AddProperty(expando, property.Name, null);
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine(obj.GetType().Name.Split('_')[0].ToLower() + "====>XProject.Domain.Entities:" + property.Name);
                                            object oData = property.GetValue(obj, null);
                                            AddProperty(expando, property.Name, ConvertToExpandoForNest(oData, level + 1));
                                        }
                                       
                                   
                                    }
                               
                            }
                            else
                            {
                                AddProperty(expando, property.Name, ((Enumeration)property.GetValue(obj, null)).DisplayName);
                            }

                        }
                        else 
                        {

                            AddProperty(expando, property.Name, ConvertToExpandoForNest(property.GetValue(obj, null), level + 1));
                        }

                    }
                }

            }
            if (propsNotMap.Where(T => T.pName == "Name").Count() > 0)
            {
                var pro = properties.Where(T => T.Name == "Name").FirstOrDefault();
                object vValue = pro.GetValue(obj, null);
                AddProperty(expando, pro.Name, vValue);
            }
            if (level<1)
            {
                foreach (var item in props)
                {
                   
                    var prMap = properties.Where(T => T.Name == item.pName).FirstOrDefault();
                    if (prMap!=null)
                    {
                        object vValue = prMap.GetValue(obj, null);
                        if (vValue != null)
                        {
                            AddProperty(expando, item.pName, ConvertToExpandoForNest(vValue, level + 1));
                        }
                        else
                        {
                            if (!listData.ContainsKey(item.p.PropertyType.ToString()))
                            {
                                listData.Add(item.p.PropertyType.ToString(), _dbContext.Set(_eastWestDbTypeLoad.Where(T => T.ToString() == item.p.PropertyType.ToString()).FirstOrDefault()).AsEnumerable().ToList());
                            }
                            var data = listData[item.p.PropertyType.ToString()];
                            var prFK = properties.Where(T => T.Name == item.aName).FirstOrDefault();
                            if (prFK != null)
                            {
                                object prFKValue = prFK.GetValue(obj, null);
                                if (prFKValue != null)
                                {
                                    vValue = data.Where(x => x.GetType().GetProperty("ID").GetValue(x, null).ToString() == prFKValue.ToString()).FirstOrDefault();
                                    if (vValue != null)
                                    {
                                        AddProperty(expando, item.pName, ConvertToExpandoForNest(vValue, level + 1));
                                    }

                                }
                            }
                        }
                    }
                   
                    

                }
            }
           
            //if (level == 0)
            {
                AddProperty(expando, "ClassForName", obj.GetType().Name.Split('_')[0].ToLower());
              //  AddProperty(expando, "KeySearch", obj.GetType().Name.Split('_')[0].ToLower());
            }

            return expando;
        }
        private void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
        private class ConnectionToES
        {
            #region Connection string to connect with Elasticsearch
            public static List<String> PostJsonDataServer(string path, String json)
            {
                List<String> listStr = new List<string>();
                foreach (var url in nodes)
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url.ToString() + path);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {

                      
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        listStr.Add(result);
                    }
                }
                return listStr;

            }

            static Uri[] nodes =ConfigurationManager.AppSettings["Elasticsearchs"].Split(',').Select(t => new Uri(t)).ToArray();
            public static ElasticClient EsClient()
            {
                ConnectionSettings connectionSettings;
                ElasticClient elasticClient;
                StaticConnectionPool connectionPool;

              

                connectionPool = new StaticConnectionPool(nodes);
                connectionSettings = new ConnectionSettings(connectionPool);
                elasticClient = new ElasticClient(connectionSettings);

                return elasticClient;
            }

            #endregion Connection string to connect with Elasticsearch
        }
    }
    public static class NestSearchHelper
    {
        
        public static IEnumerable<object> AsEnumerable(this DbSet set)
        {
           
            foreach (var entity in set)
                yield return entity;
           
        }

        public static string ToStringCS(this DateTime obj)
        {
            return obj.ToString("dd/MM/yyyy HH:mm:ss");
        }
        public static string ToStringCS(this float obj)
        {
            return obj.ToString("N2");
        }
        public static string ToStringCS(this double obj)
        {
            return obj.ToString("N2");
        }
        public static string ToStringCS(this decimal obj)
        {
            return obj.ToString("N2");
        }
        public static string ToStringCS(this object obj)
        {
            if (obj.GetType().ToString().ToLower().Contains("datetime"))
            {
               return  ((DateTime) obj).ToStringCS();
            }
            if (obj.GetType().ToString().ToLower().Contains("decimal"))
            {
                return ((decimal)obj).ToStringCS();
            }
            if (obj.GetType().ToString().ToLower().Contains("double"))
            {
                return ((double)obj).ToStringCS();
            }
            if (obj.GetType().ToString().ToLower().Contains("float"))
            {
                return ((float)obj).ToStringCS();
            }
           
            return obj.ToString();
        }
    }



}