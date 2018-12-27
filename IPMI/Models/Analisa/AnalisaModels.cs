using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPMI.Models.Analisa
{
    public class AnalisaModels
    {
        public int id { get; set; }
        public string NoIPMI { get; set; }
        public string Penyebab { get; set; }
        public string RPerbaikan { get; set; }
        public string Target { get; set; }
        public string PIC { get; set; }
        public string TglActual { get; set; }
        public string Tgl { get; set; }
        public string Dari { get; set; }
        public string Masalah { get; set; }
        public string Status { get; set; }
        public string InputByDept { get; set; }
        public string UserName { get; set; }
    }
}