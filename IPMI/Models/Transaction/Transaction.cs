using System.Data.SqlClient;

namespace IPMI.Models.Transaction
{

    public class TransactionRequest
    {
        public string RequestID { get; set; }
        public string RequestDate { get; set; }
        public string ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string NoReff { get; set; }
        public string NoCash { get; set; }
        public string TotalRequest { get; set; }
        public string Status { get; set; }
        public string Maker { get; set; }
        public string RequestDetailID { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string LevelName { get; set; }
        public string Approve { get; set; }
        public string ActionDate { get; set; }
        public string ActionID { get; set; }
        public string RequestActionID { get; set; }
        public string LevelID { get; set; }
        public string Comment { get; set; }
        

    }
}
