using System.Data.SqlClient;

namespace IPMI.Models.Master
{

    public class MasterAction
    {
        public string ActionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class MasterGroup
    {
        public string GroupID { get; set; }
        public string Name { get; set; }
    }
    public class MasterLevel
    {
        public string LevelID { get; set; }
        public string Name { get; set; }
    }
    public class MasterProcess
    {
        public string ProcessID { get; set; }
        public string Name { get; set; }
        public string Requester { get; set; }
        public string ProcessDetailID { get; set; }
        public string UserID { get; set; }
        public string Approve { get; set; }
    }
    public class MasterUser
    {
        public string UserID { get; set; }
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public string LevelID { get; set; }
        public string LevelName { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
    }
    public class PartnerModel
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
    }
}
