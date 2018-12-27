using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IPMI.Models.Master
{
    public class deptModels
    {
        [Key]
        public string IdDept { get; set; }
        public string NamaDept { get; set; }
    }
}