using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XProject.Web.Areas.Admin.Models
{
    public class UploadModel
    {
        public string NameField { get; set; }
        public string LabelField { get; set; }
        public string ValueField { get; set; }
        public string ValueFieldUrl { get; set; }
        public UploadModel(string NameField, string ValueField,string LabelField)
        {
            this.NameField = NameField;
            this.ValueField = ValueField;
            this.LabelField = LabelField;
        } public UploadModel(string NameField, string ValueField, string ValueFieldUrl, string LabelField)
        {
           
            this.NameField = NameField;
            this.ValueField = ValueField;
            this.LabelField = LabelField;
            this.ValueFieldUrl = ValueFieldUrl;
        }
    }
}