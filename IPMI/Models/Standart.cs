using System.Data.SqlClient;

namespace IPMI.Models
{

    public class ReportFilter
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
    }
    public class StandartComboBox
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class TransactionHelper
    {
        SqlCommand _Cmd = new SqlCommand();
        public SqlCommand Command
        {
            get { return _Cmd; }
            set { _Cmd = value; }
        }
    }
}
