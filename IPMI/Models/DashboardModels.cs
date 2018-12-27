using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPMI.Models
{
    public class DashboardModels
    {
        public int Open { get; set; }
        public int Closed { get; set; }
        public int Total { get; set; }
        public int Global { get; set; }
        public int Verifikasi { get; set; }
        public int OpenA { get; set; }
        public int ClosedA { get; set; }
        public int TotalA { get; set; }

        public int OnProgressA { get; set; }

        
    }

    public class Pie
    {
        public string ND12 { get;set;}
        public string BOD1 { get; set; }
        public string ENG3 { get; set; }
        public string ENG1 { get; set; }
        public string FAC3 { get; set; }
        public string FAC1 { get; set; }
        public string HRD3 { get; set; }
        public string HRD1 { get; set; }

        public string INJ3 { get; set; }
        public string INJ5 { get; set; }
        public string INJ1 { get; set; }
        public string MKT3 { get; set; }
        public string MKT1 { get; set; }
        public string MIS3 { get; set; }
        public string MIS1 { get; set; }
        public string NPD3 { get; set; }
        public string NPD1 { get; set; }
        public string PP32 { get; set; }
        public string PP52 { get; set; }
        public string PP12 { get; set; }
        public string PGA3 { get; set; }
        public string PGA1 { get; set; }
        public string PAB3 { get; set; }
        public string DLA3 { get; set; }

        public string DLB3 { get; set; }
        public string PPC3 { get; set; }
        public string P3 { get; set; }
        public string PPC5 { get; set; }
        public string PAB1 { get; set; }
        public string PDL1 { get; set; }
        public string PPC1 { get; set; }
        public string P1 { get; set; }
        public string PUR3 { get; set; }
        public string PVM3 { get; set; }
        public string PUR1 { get; set; }
        public string PVM1 { get; set; }
        public string QCA3 { get; set; }
        public string QOG3 { get; set; }
        public string QQA3 { get; set; }
        public string QCA1 { get; set; }
        public string QOG1 { get; set; }
        public string QQA1 { get; set; }
        public string QSD3 { get; set; }
        public string QSD1 { get; set; }
    }

    public class BarChart
    {
        public string IdDept { get; set; }
        public int Open { get; set; }
        public int Closed { get; set; }
    }
}