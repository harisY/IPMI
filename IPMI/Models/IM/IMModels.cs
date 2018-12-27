using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IPMI.Models.IM
{
    public class IMModels
    {
        public int id { get; set; }
        [Key]
        public string NoIPMI { get; set; }
        public string Tgl { get; set; }
        public string Dari { get; set; }
        public string Ke { get; set; }
        public string NamaDept { get; set; }
        public string Masalah { get; set; }
        public int Jumlah { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
       public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}