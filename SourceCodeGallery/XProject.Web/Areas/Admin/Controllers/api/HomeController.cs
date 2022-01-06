//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web;
//using System.Web.Http;
//using System.Windows.Forms;
//using Newtonsoft.Json;
//using XProject.Domain.Abstract;
//using XProject.Domain.Entities;
//using XProject.Web.Areas.Admin.Models;
//using XProject.Web.Infrastructure;

//namespace XProject.Web.Areas.Admin.Controllers.api
//{
//    public class HomeController : ApiController
//    {
//        [HttpGet, ActionName("CategoryGet")]
//        public HttpResponseMessage CategoryGet()
//        {
//            var stateRepository = DependencyHelper.GetService<IGeneralRepository<NhCategory>>();
//            string q = HttpContext.Current.Request.QueryString["q"];//keyword
//            int page = HttpContext.Current.Request.QueryString["page"] != null ? int.Parse(HttpContext.Current.Request.QueryString["page"]) : 1;
//            int pagesize = HttpContext.Current.Request.QueryString["pagesize"] != null ? int.Parse(HttpContext.Current.Request.QueryString["pagesize"]) : 20;
//            int total = 0;
//            // var dt = dto.tblProjects.Where(i => i.Title.Contains(q)).Select(i => new { id = i.ID.ToString(), text = i.Title }).AsQueryable();
//            string query = "select @columns from NhCategories s  where s.Active=1 and  s.Name like N'%" + q + "%' collate SQL_Latin1_General_CP1_CI_AI";
//            total = int.Parse(stateRepository.ExecuSql_Dataset(query.Replace("@columns", "count(*)")).Tables[0].Rows[0][0].ToString());
//            query += " order by s.id OFFSET " + pagesize * (page - 1) + " ROWS FETCH NEXT " + pagesize + " ROWS ONLY";

//            var dt = stateRepository.ExecuSql_Dataset(query.Replace("@columns", "s.ID 'id', s.Name'text'")).Tables[0];


//            string json = JsonConvert.SerializeObject(new { results = dt, countitem = total }, Formatting.Indented);

//            if (!string.IsNullOrEmpty(json))
//            {
//                var response = Request.CreateResponse(HttpStatusCode.OK);
//                response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
//                return response;
//            }
//            throw new HttpResponseException(HttpStatusCode.NotFound);
//        }
//        [HttpGet, ActionName("CategoryGet")]
//        public HttpResponseMessage CategoryGet(int id)
//        {
//            string json = "";
//            var stateRepository = DependencyHelper.GetService<IGeneralRepository<NhCategory>>();
//            var o = new SelectIntModel();
          
//                string query1 = "select s.ID 'id', s.Name 'text' from NhCategories s where s.ID = " + id;

//                var dt = stateRepository.ExecuSql_Dataset(query1).Tables[0];
//                 o = new SelectIntModel
//                {
//                    id = int.Parse(dt.Rows[0]["id"].ToString()),
//                    text = dt.Rows[0]["text"].ToString()
//                };
//                json = JsonConvert.SerializeObject(o, Formatting.Indented);
            
            
//            if (!string.IsNullOrEmpty(json))
//            {
//                var response = Request.CreateResponse(HttpStatusCode.OK);
//                response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
//                return response;
//            }
//            throw new HttpResponseException(HttpStatusCode.NotFound);
//        }



//        [HttpGet, ActionName("CustomerGet")]
//        public HttpResponseMessage CustomerGet()
//        {
//            var stateRepository = DependencyHelper.GetService<IGeneralRepository<NhCustomer>>();
//            string q = HttpContext.Current.Request.QueryString["q"];//keyword
//            int page = HttpContext.Current.Request.QueryString["page"] != null ? int.Parse(HttpContext.Current.Request.QueryString["page"]) : 1;
//            int pagesize = HttpContext.Current.Request.QueryString["pagesize"] != null ? int.Parse(HttpContext.Current.Request.QueryString["pagesize"]) : 20;
//            int total = 0;
//            // var dt = dto.tblProjects.Where(i => i.Title.Contains(q)).Select(i => new { id = i.ID.ToString(), text = i.Title }).AsQueryable();
//            string query = "select @columns from NhCustomers s  where s.Active=1 and s.Code+' - '+ s.Name like N'%" + q + "%' collate SQL_Latin1_General_CP1_CI_AI";
//            total = int.Parse(stateRepository.ExecuSql_Dataset(query.Replace("@columns", "count(*)")).Tables[0].Rows[0][0].ToString());
//            query += " order by s.id OFFSET " + pagesize * (page - 1) + " ROWS FETCH NEXT " + pagesize + " ROWS ONLY";

//            var dt = stateRepository.ExecuSql_Dataset(query.Replace("@columns", "s.ID 'id', s.Code +' - '+ s.Name  'text'")).Tables[0];


//            string json = JsonConvert.SerializeObject(new { results = dt, countitem = total }, Formatting.Indented);

//            if (!string.IsNullOrEmpty(json))
//            {
//                var response = Request.CreateResponse(HttpStatusCode.OK);
//                response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
//                return response;
//            }
//            throw new HttpResponseException(HttpStatusCode.NotFound);
//        }
//        [HttpGet, ActionName("CustomerGet")]
//        public HttpResponseMessage CustomerGet(int id)
//        {
//            string json = "";
//            var stateRepository = DependencyHelper.GetService<IGeneralRepository<NhCustomer>>();
//            var o = new SelectIntModel();

//            string query1 = "select s.ID 'id',s.Code+' - '+ s.Name 'text' from NhCustomers s where s.ID = " + id;

//            var dt = stateRepository.ExecuSql_Dataset(query1).Tables[0];
//            o = new SelectIntModel
//            {
//                id = int.Parse(dt.Rows[0]["id"].ToString()),
//                text = dt.Rows[0]["text"].ToString()
//            };
//            json = JsonConvert.SerializeObject(o, Formatting.Indented);


//            if (!string.IsNullOrEmpty(json))
//            {
//                var response = Request.CreateResponse(HttpStatusCode.OK);
//                response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
//                return response;
//            }
//            throw new HttpResponseException(HttpStatusCode.NotFound);
//        }


//        [HttpGet, ActionName("SuppliersGet")]
//        public HttpResponseMessage SuppliersGet()
//        {
//            var stateRepository = DependencyHelper.GetService<IGeneralRepository<NhCustomer>>();
//            string q = HttpContext.Current.Request.QueryString["q"];//keyword
//            int page = HttpContext.Current.Request.QueryString["page"] != null ? int.Parse(HttpContext.Current.Request.QueryString["page"]) : 1;
//            int pagesize = HttpContext.Current.Request.QueryString["pagesize"] != null ? int.Parse(HttpContext.Current.Request.QueryString["pagesize"]) : 20;
//            int total = 0;
//            // var dt = dto.tblProjects.Where(i => i.Title.Contains(q)).Select(i => new { id = i.ID.ToString(), text = i.Title }).AsQueryable();
//            string query = "select @columns from NhSuppliers s  where s.Active=1 and s.Code+' - '+ s.Name like N'%" + q + "%' collate SQL_Latin1_General_CP1_CI_AI";
//            total = int.Parse(stateRepository.ExecuSql_Dataset(query.Replace("@columns", "count(*)")).Tables[0].Rows[0][0].ToString());
//            query += " order by s.id OFFSET " + pagesize * (page - 1) + " ROWS FETCH NEXT " + pagesize + " ROWS ONLY";

//            var dt = stateRepository.ExecuSql_Dataset(query.Replace("@columns", "s.ID 'id', s.Code +' - '+ s.Name  'text'")).Tables[0];


//            string json = JsonConvert.SerializeObject(new { results = dt, countitem = total }, Formatting.Indented);

//            if (!string.IsNullOrEmpty(json))
//            {
//                var response = Request.CreateResponse(HttpStatusCode.OK);
//                response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
//                return response;
//            }
//            throw new HttpResponseException(HttpStatusCode.NotFound);
//        }
//        [HttpGet, ActionName("SuppliersGet")]
//        public HttpResponseMessage SuppliersGet(int id)
//        {
//            string json = "";
//            var stateRepository = DependencyHelper.GetService<IGeneralRepository<NhCustomer>>();
//            var o = new SelectIntModel();

//            string query1 = "select s.ID 'id',s.Code+' - '+ s.Name 'text' from NhSuppliers s where s.ID = " + id;

//            var dt = stateRepository.ExecuSql_Dataset(query1).Tables[0];
//            o = new SelectIntModel
//            {
//                id = int.Parse(dt.Rows[0]["id"].ToString()),
//                text = dt.Rows[0]["text"].ToString()
//            };
//            json = JsonConvert.SerializeObject(o, Formatting.Indented);


//            if (!string.IsNullOrEmpty(json))
//            {
//                var response = Request.CreateResponse(HttpStatusCode.OK);
//                response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
//                return response;
//            }
//            throw new HttpResponseException(HttpStatusCode.NotFound);
//        }
//    }
//}
