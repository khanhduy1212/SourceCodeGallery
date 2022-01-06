using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XProject.Domain.Entities;

namespace XProject.Web.Models
{
    public class AllModel
    {
       
        public List<GeoModel> ListGeoModel { get; set; }
        public List<XMenu> ListMenu { get; set; }
        public XMenu TblMenu { get; set; }


        public string Cfile { get; set; }
        public string nameImg { get; set; }
        public int? TypeUpdate { get; set; }
    }
    public class GeoModel
    {

        public int CityId { get; set; }
        public string CityName { get; set; }
        public int DistId { get; set; }
        public string DistName { get; set; }
        public int WardId { get; set; }
        public string WardName { get; set; }
        public string LangugeId { get; set; }

    }
    public class InputImage
    {

        public bool showCaption { get; set; }
        public bool showRemove { get; set; }
        public bool showUpload { get; set; }
        public bool overwriteInitial { get; set; }
        public bool initialPreviewAsData { get; set; }
        public string[] initialPreview { get; set; }
       

    }


}