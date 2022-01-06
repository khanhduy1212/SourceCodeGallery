using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace XProject.Domain.Entities
{
    public class XPicture : XBaseEntiry
    {
        public int? ProductsId { get; set; }
        public string OriginalFilepath { get; set; }
        public byte? Position { get; set; }
        public DateTime? Converted { get; set; }
        public string ConvertedFilename { get; set; }
        public bool? ToCheck { get; set; }
        public bool? Isvalidated { get; set; }
        public string ConvertedFilename90 { get; set; }
        public string ConvertedFilename180 { get; set; }
        public string ConvertedFilename270 { get; set; }
        public byte? AngleType { get; set; }
        public int? TypeId { get; set; }
        public string Title { get; set; }
        public string ProductsIdTmp { get; set; }

    }
}
