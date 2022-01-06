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
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using XProject.Domain;
using XProject.Domain.Entities;
using Elasticsearch.Net;
using Elasticsearch.Net.ConnectionPool;

using Nest;
using Newtonsoft.Json;
using Ninject.Infrastructure.Language;
using NS;
using ServiceStack;

namespace XProject.Web.Areas.Admin.Helper
{
    
    public class NestSearch
    {
        public  readonly string Project = "EastWestSearch";
        private List<PropertyInfo> _eastWestDbSetProperties;
        private DbContext _dbContext;
        private List<Type> _eastWestDbType;
        private Dictionary<String, List<Object>> listData = new Dictionary<String, List<object>>();
        private Dictionary<String, bool> checkDataSync = new Dictionary<String, bool>();
        List<ExpandoObject> listErrorData=new List<ExpandoObject>(); 
        private List<Type> _eastWestDbTypeLoad;
        private List<PropertyInfo> _eastWestDbSetPropertiesLoad;
       private static List<String> _listController =new List<string>();
        public NestSearch(DbContext context)
        {
            _listController = XProject.Web.Infrastructure.Utility.MvcHelper.GetControllers().Where(T => T.Namespace.ToLower() == "XProject.Web.Areas.Admin.Controllers".ToLower()).Select(T => T.Name.Replace("Controller", "")).ToList();
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

        public NestSearch(DbContext context,string filter="all")
        {
            _listController = XProject.Web.Infrastructure.Utility.MvcHelper.GetControllers().Where(T => T.Namespace.ToLower() == "XProject.Web.Areas.Admin.Controllers".ToLower()).Select(T => T.Name.Replace("Controller", "")).ToList();
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
                    if (filter == "all" || setType.GetInterface(typeof(IDbSet<>).FullName).GenericTypeArguments()[0].Name.ToLower().IndexOf(filter) > -1)
                    {
                        _eastWestDbType.Add(setType.GetInterface(typeof(IDbSet<>).FullName).GenericTypeArguments()[0]);
                        _eastWestDbSetProperties.Add(property);
                   //     Debug.WriteLine(setType.GetInterface(typeof(IDbSet<>).FullName).GenericTypeArguments()[0].Name.ToLower());

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

            bool status;
       
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
            else
            {
                status = false;
                listErrorData.Add(expandoObject);
            }
            Debug.WriteLine("insertData:" + (expandoObject as IDictionary<string, object>)["ClassForName"].ToString() + " - " + (expandoObject as IDictionary<string, object>)["ID"].ToString() + " is " + (status ? "success" : "error"));
            return status;
        }

        public List<dynamic> SearchFullText(String query, string[] types, string[] typeNames, string[] columnNames, string[] columnNameViews, string[] sortColumns, string[] sortColumnDirs,int start=0,int length=10)
        {
            List<dynamic> listDataResponse = new List<dynamic>();
            MultiSearchDescriptor descriptor = new MultiSearchDescriptor(); ;
            for (int i = 0; i < types.Length; i++)
            {
                String index = types[i];
                String sort = "iD";
                if (sortColumns.Length>i)
                {
                    sort = sortColumns[i];
                }
                String sortDir = "asc";
                if (sortColumnDirs.Length > i)
                {
                    sortDir = sortColumnDirs[i];
                }
                String columns = "";
                if (columnNames.Length > i)
                {
                    columns = columnNames[i];
                }
                if (columns=="")
                {
                    columns = "iD";
                }
                Type type= _eastWestDbTypeLoad.Where(t => t.Name.ToLower().IndexOf(index.Trim().ToLower()) > -1).FirstOrDefault();
                if (type!=null)
                {
                    List<string> columnTextSearchs = GetColumnFulltextSearch(type);
                    columnTextSearchs = columnTextSearchs.Union(columns.Split(',')).ToList();
                    columns=String.Join(",", columnTextSearchs);
                }
              
                descriptor=descriptor.Search<dynamic>(index, s => s
                    .Index(index)
                    .From(start)
                    .Take(length)
                    .DefaultOperator(DefaultOperator.Or)
                    .Sort(st => sortDir == "asc" ? st.OnField(sort).Ascending() : st.OnField(sort).Descending())
                    .Type(Project)
                    /*.Highlight(h=>
                    h.OnFields(f=>
                       f.OnField("*")
                       .Encoder("html").PreTags("<hl>").PostTags("</hl>")
                       .HighlightQuery(q =>
                          q.QueryString(qs => qs.OnFields(columns.Split(',')).DefaultOperator(Operator.And).Query(query)))))*/
                    .Query(q => q.QueryString(qs => qs.OnFields(columns.Split(',')).DefaultOperator(Operator.And).Query(query)) && BuildDeleteQuery("False".Split(','))));
            }
            var responses = ConnectionToES.EsClient().MultiSearch(descriptor);
            for (int i = 0; i < types.Length; i++)
            {
               var data  = responses.GetResponse<dynamic>(types[i]);
                List<Dictionary<string,object>> listData=new List<Dictionary<string, object>>();
                 foreach (var item in data.Hits)
                   {
                     var obj = GetObjectFromFields(item.Source, columnNames[i].Split(','));
                     listData.Add(obj);
                   }
               
               var rtModel = new
               {
                   query = query,
                   typeName = typeNames[i],
                   type = types[i],
                   columnName = columnNames[i],
                   columnNameView = columnNameViews[i],
                   sort = sortColumns[i],
                   sortDir = sortColumnDirs[i],
                   total = data.Total,
                   datas = listData,
                   start=start,
                   length=length
                   //  data=datas[0].Hits.Select("")

               };
               listDataResponse.Add(rtModel);
            }





            return listDataResponse;
        }

        public static string GetController(string name)
        {
            var nameFind = _listController.Where(t => t.ToLower().Trim().IndexOf(name.Trim().ToLower()) > -1).FirstOrDefault();
            return nameFind;
        }
        public QueryContainer BuildDeleteQuery(String[] list)
        {

            QueryContainer container1 = null;
            for (int i = 0; i < list.Count(); i++)
            {
                if (container1 != null)
                {
                    container1 |= new QueryContainer(new MatchQuery() { Field = new PropertyPathMarker() { Name = "isDeleted_string" }, Query = list[i] });
                }
                else
                {
                    container1 = new QueryContainer(new MatchQuery() { Field = new PropertyPathMarker() { Name = "isDeleted_string" }, Query = list[i] });
                }
            }
            return container1;
        }

        public List<dynamic> AjaxAutoComplete(string query,string listIndex)
        {
            List<dynamic> listData=new List<dynamic>();
           var rs=  ConnectionToES.EsClient().Search<dynamic>(qe => qe.Indices(listIndex.Split(','))
                .Type("EastWestSearch")
                .Take(10)
                .From(0)
                .Sort(t => t.OnField("iD").Ascending())
                .Query(q => q.QueryString(qs => qs.OnFields("name".Split(',')).Query(query + "*")) && BuildDeleteQuery("False".Split(','))));
           if (rs.Hits.Count()>0)
            {
                foreach (var item in rs.Hits)
                {

                    listData.Add(new
                    {
                        value = item.Id,
                        label = ((dynamic)item.Source).name.Value,
                        nameLink = GetController(((dynamic)item.Source).classForName.Value),
                    });
                }
            }
            return listData;
        }
        void SetUpTypeSearch(ref Dictionary<string,object> dicRef)
        {
            foreach (var item in dicRef.Keys.AsEnumerable().ToList())
            {
                if (dicRef[item] is Dictionary<string, object>)
                {
                    if (((Dictionary<string, object>)dicRef[item]).ContainsKey("type") && ((Dictionary<string, object>)dicRef[item])["type"].ToString()=="text")
                    {
                        Dictionary<string, object> objects = new Dictionary<string, object>();
                        objects.Add("type", "text");
                        objects.Add("analyzer", "include_special_character");
                        dicRef[item] = objects; 
                         
                          
                    }
                    else
                    {
                        var dictionary = (Dictionary<string, object>)dicRef[item];
                        SetUpTypeSearch(ref dictionary);
                        dicRef[item] = dictionary;
                    }
                }
                
               
            }
        }
        public void SyncSearchData()
        {

            listData.Clear();
            listErrorData.Clear();
            for (int i = 0; i < _eastWestDbSetProperties.Count; i++)
            {
                String index = "";
                if (!listData.ContainsKey(_eastWestDbType[i].ToString()))
                {
                    listData.Add(_eastWestDbType[i].ToString(), _dbContext.Set(_eastWestDbType[i]).AsEnumerable().ToList());
                }
                var data = listData[_eastWestDbType[i].ToString()];
                if (data.Count>0)
                {
                    foreach (var item in data)
                    {
                        var dataETD = ConvertToExpandoForNest(item);
                        if ((dataETD as IDictionary<string, object>).ContainsKey("isDeleted"))
                        {
                            if ((dataETD as IDictionary<string, object>)["isDeleted"] != null && (bool)(dataETD as IDictionary<string, object>)["isDeleted"] == false)
                            {
                                index = (dataETD as IDictionary<string, object>)["ClassForName"].ToString();
                               
                            }
                           
                        }
                        else
                        {
                            index = (dataETD as IDictionary<string, object>)["ClassForName"].ToString();
                         
                        }
                        if (index!="")
                        {
                            if (ConnectionToES.EsClient().IndexExists(index).Exists)
                            {
                                ConnectionToES.EsClient().DeleteIndex(index);
                            }
                            InsertData(dataETD);
                            break;
                        }
                       
                    }

                    List<String> strList = ConnectionToES.GetJsonDataServer(index + "/_mappings");

                    JavaScriptSerializer deserializer = new JavaScriptSerializer();

                    Dictionary<string, object> values = deserializer.Deserialize<Dictionary<string, object>>(strList[0]);



                    SetUpTypeSearch(ref values);


                    if (ConnectionToES.EsClient().IndexExists(index).Exists)
                    {
                        ConnectionToES.EsClient().DeleteIndex(index);
                    }

                    String strAnalyzer =
                        "{\"analysis\": {\"filter\": {\"my_ascii_folding\": {\"type\" : \"asciifolding\",\"preserve_original\": \"true\"}},\"analyzer\": {\"include_special_character\": {\"type\":  \"custom\",\"filter\": [\"lowercase\",\"my_ascii_folding\"],\"tokenizer\": \"whitespace\"}}}}";

                    ((Dictionary<string, object>)values[index]).Add("settings", deserializer.Deserialize<Dictionary<string, object>>(strAnalyzer));

                    StringBuilder dataOutput = new StringBuilder();
                    deserializer.Serialize(values[index], dataOutput);

                    String dataString = dataOutput.ToString();
                    strList = ConnectionToES.PostJsonDataServer(index, dataString);
                    foreach (var str in strList)
                    {
                      //  Debug.WriteLine(str);
                    }
                }
                
              
            }

            for (int i = 0; i < _eastWestDbSetProperties.Count; i++)
            {

                var data = _dbContext.Set(_eastWestDbType[i]).AsEnumerable().ToList();
            
                new Thread((dynamic obj) =>
                {

                    List<object> datas = obj.GetType().GetProperty("data").GetValue(obj, null);
                    string type = obj.GetType().GetProperty("type").GetValue(obj, null);
                    checkDataSync.Add(type, false);
                    foreach (var item in datas)
                    {
                        try
                        {
                            var dataETD = ConvertToExpandoForNest(item);
                            InsertData(dataETD);
                        }
                        catch (Exception)
                        {
                            
                        }
                       
                    
                    }
                    checkDataSync[type] = true;
                    SyncError();
                }).Start(new { data = data, type = _eastWestDbType[i].ToString() });
               
            }
        }

        private void SyncError()
        {
            if (listErrorData.Count>0)
            {
                List<ExpandoObject> objcList=new List<ExpandoObject>();
                foreach (var item in listErrorData)
                {
                    objcList.Add(item);
                }
                foreach (var item in objcList)
                {
                    listErrorData.Remove(item);
                }
                foreach (var item in objcList)
                {
                    InsertData(item);
                }
            }
        }
       

        private Dictionary<String,object> GetObjectFromFields(dynamic obj,String[] strFields)
        {
            Dictionary<String,object> objects=new Dictionary<string, object>();
            foreach (var item in strFields)
            {
                objects.Add(item, GetDeepPropertyValue(obj, item));
            }

            return objects;
        }
        private object GetDeepPropertyValue(object instance, string path)
        {
            if (instance!=null)
            {
                var objSelect = (instance as Newtonsoft.Json.Linq.JObject).SelectToken(path);
                if (objSelect is Newtonsoft.Json.Linq.JValue)
                {
                    return
                        ((Newtonsoft.Json.Linq.JValue)objSelect).Value;
                }
                return objSelect;
            }
            return null;

        }
        
        private List<String> GetColumnFulltextSearch(Type obj)
        {
            List<String> listName=new List<string>();
            var props = obj.GetProperties().Where(
               prop => Attribute.IsDefined(prop, typeof(System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute))
               ).Select(prop => new { p = prop, pName = prop.Name, a = Attribute.GetCustomAttribute(prop, typeof(System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute)), aName = ((System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute)Attribute.GetCustomAttribute(prop, typeof(System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute))).Name });

            var propsNotMap = obj.GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute))
                ).Select(prop => new { p = prop, pName = prop.Name, a = Attribute.GetCustomAttribute(prop, typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute)) });
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] properties = obj.GetProperties(flags);

            foreach (PropertyInfo property in properties)
            {

                if (props.Where(T => T.aName == property.Name).ToList().Count == 0 && props.Where(T => T.pName == property.Name).ToList().Count == 0 && propsNotMap.Where(T => T.pName == property.Name).ToList().Count == 0)
                {
                    if (!(property.PropertyType.GenericTypeArguments().Length > 0 && property.PropertyType.GetGenericTypeDefinition().Namespace == "System.Collections.Generic"))
                    {
                        if (!property.GetGetMethod().IsVirtual)
                        {
                            if (!property.PropertyType.IsSubclassOf(typeof(Enumeration)))
                            {
                                if (property.PropertyType.Namespace.IndexOf("XProject.Domain.Entities") == -1)
                                {
                                    string nameType = property.PropertyType.ToString();
                                    string[] nameTypesAccept = new string[] { "long", "datetime", "int", "float", "double", "decimal", "boolean", "enum" };
                                    if (nameTypesAccept.Where(t => nameType.ToLower().Contains(t)).Count() > 0)
                                    {
                                        listName.Add(property.Name.Substring(0,1).ToLower()+property.Name.Substring(1)+"_string");
                                    }
                                    else
                                    {
                                      
                                        listName.Add(property.Name.Substring(0, 1).ToLower() + property.Name.Substring(1));
                                       
                                        
                                    }
                                }
                                
                            }
                            else
                            {
                                listName.Add(property.Name.Substring(0, 1).ToLower() + property.Name.Substring(1));
                            }
                        }
                    }
                }
            }
            return listName;
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
            if (propsNotMap.Where(T=>T.pName=="Name").Count()>0)
            {
                var pro= properties.Where(T => T.Name == "Name").FirstOrDefault();
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
            public static List<String> BulkJsonDataServer(string path, String json)
            {
                List<String> listStr = new List<string>();
                foreach (var url in nodes)
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url.ToString() + path);
                    httpWebRequest.ContentType = "application/x-ndjson";
                    httpWebRequest.Method = "POST";

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
            public static List<String> GetJsonDataServer(string path)
            {
                List<String> listStr = new List<string>();
                foreach (var url in nodes)
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url.ToString() + path);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "GET";


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

                //Connection string for Elasticsearch
                /*connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200/")); //local PC
                elasticClient = new ElasticClient(connectionSettings);*/

                //Multiple node for fail over (cluster addresses)
               

                connectionPool = new StaticConnectionPool(nodes);
                connectionSettings = new ConnectionSettings(connectionPool);
                elasticClient = new ElasticClient(connectionSettings);

                return elasticClient;
            }

            #endregion Connection string to connect with Elasticsearch
        }
    }
    /*public static class NestSearchHelper
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
    }*/



}