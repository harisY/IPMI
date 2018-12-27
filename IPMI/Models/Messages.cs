using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPMI.Models
{
    public class Messages
    {
        public int MessageID { get; set; }

        public string Message { get; set; }

        public string EmptyMessage { get; set; }

        public DateTime MessageDate { get; set; }
        public string Jumlah { get; set; }
    }
}
