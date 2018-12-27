using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using IPMI.Models;
using IPMI.Models.Master;
using LibDataAccess;

namespace IPMI.Master
{
    public class MasterRepository
    {
        MyLib dbAccess = new MyLib();
        string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        //=========================> Action <=========================
        public List<MasterAction> GetListActions()
        {
            List<MasterAction> Result = new List<MasterAction>();
            string Query = @"SELECT * FROM tbl_action WHERE ActionID <> '0' ORDER BY ActionID";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new MasterAction
                    {
                        ActionID  = Convert.ToString(dr["ActionID"]),
                        Name = Convert.ToString(dr["Name"]),
                        Description = Convert.ToString(dr["Description"])
                    });
            }
            return Result;
        }

        public bool AddAction(string Name, string Desc)
        {
            bool result = false;
            try
            {
                string Query = @"INSERT INTO tbl_action (Name, Description)
                                VALUES ('" + Name + "', '" + Desc + "')";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                if (res != -1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        public bool EditAction(string Id, string Name, string Desc)
        {
            bool result = false;
            try
            {
                string Query = @"UPDATE tbl_action SET Name = '" + Name + "', Description  = '" + Desc + "' WHERE ActionID = '" + Id + "'";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                if (res != -1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }
        
        public bool DeleteAction(string Id)
        {
            bool result = false;
            try
            {
                string Query = @"DELETE FROM tbl_action WHERE ActionID = '" + Id + "'";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                if (res != -1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }


        //=========================> Group <=========================
        public List<MasterGroup> GetListGroups()
        {
            List<MasterGroup> Result = new List<MasterGroup>();
            string Query = @"SELECT * FROM tbl_group ORDER BY GroupID";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new MasterGroup
                    {
                        GroupID = Convert.ToString(dr["GroupID"]),
                        Name = Convert.ToString(dr["Name"]),
                    });
            }
            return Result;
        }

        public bool AddGroup(string Name)
        {
            bool result = false;
            try
            {
                string Query = @"INSERT INTO tbl_group (Name)
                                VALUES ('" + Name + "')";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                if (res != -1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        public bool EditGroup(string Id, string Name)
        {
            bool result = false;
            try
            {
                string Query = @"UPDATE tbl_group SET Name = '" + Name + "' WHERE GroupID = '" + Id + "'";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                if (res != -1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        public bool DeleteGroup(string Id)
        {
            bool result = false;
            try
            {
                string Query = @"DELETE FROM tbl_group WHERE GroupID = '" + Id + "'";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                if (res != -1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        //=========================> Level <=========================
        public List<MasterLevel> GetListLevels()
        {
            List<MasterLevel> Result = new List<MasterLevel>();
            string Query = @"SELECT * FROM tbl_level ORDER BY LevelID";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new MasterLevel
                    {
                        LevelID = Convert.ToString(dr["LevelID"]),
                        Name = Convert.ToString(dr["Name"]),
                    });
            }
            return Result;
        }

        public bool AddLevel(string Name)
        {
            bool result = false;
            try
            {
                string Query = @"INSERT INTO tbl_level (Name)
                                VALUES ('" + Name + "')";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                if (res != -1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        public bool EditLevel(string Id, string Name)
        {
            bool result = false;
            try
            {
                string Query = @"UPDATE tbl_level SET Name = '" + Name + "' WHERE LevelID = '" + Id + "'";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                if (res != -1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        public bool DeleteLevel(string Id)
        {
            bool result = false;
            try
            {
                string Query = @"DELETE FROM tbl_level WHERE LevelID = '" + Id + "'";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                if (res != -1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        //=========================> Process <=========================

        public List<StandartComboBox> GetUsers()
        {
            List<StandartComboBox> Result = new List<StandartComboBox>();
            string Query = @"SELECT UserName as Value, NameIdentifier  as Text FROM AspNetUsers ORDER BY NameIdentifier ASC";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new StandartComboBox
                    {
                        Value = Convert.ToString(dr["Value"]),
                        Text = Convert.ToString(dr["Text"])
                    });
            }
            return Result;

        }

        public List<MasterProcess> GetListProcesss()
        {
            List<MasterProcess> Result = new List<MasterProcess>();
            string Query = @"SELECT P.ProcessID, P.Name, P.Requester, U.NameIdentifier FROM tbl_process P LEFT OUTER JOIN AspNetUsers U ON P.Requester = U.UserName ORDER BY P.ProcessID";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new MasterProcess
                    {
                        ProcessID = Convert.ToString(dr["ProcessID"]),
                        Name = Convert.ToString(dr["Name"]),
                        UserID = Convert.ToString(dr["Requester"]),
                        Requester = Convert.ToString(dr["NameIdentifier"])
                    });
            }
            return Result;
        }

        public List<MasterProcess> GetListProcessDetail(string Id)
        {
            List<MasterProcess> Result = new List<MasterProcess>();
            string Query = @"SELECT P.ProcessDetailID, P.ProcessID, P.UserName, U.NameIdentifier FROM tbl_process_detail P LEFT OUTER JOIN AspNetUsers U ON P.UserName = U.UserName WHERE P.ProcessID = '" + Id + "' ORDER BY P.ProcessDetailID";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new MasterProcess
                    {
                        ProcessDetailID = Convert.ToString(dr["ProcessDetailID"]),
                        ProcessID = Convert.ToString(dr["ProcessID"]),
                        UserID = Convert.ToString(dr["UserName"]),
                        Approve = Convert.ToString(dr["NameIdentifier"])
                    });
            }
            return Result;
        }

        public bool AddProcess(string Name, string Requester, string Flags, string Approves)
        {
            bool result = false;
            var Flag = "";
            var Approve = "";
            var ProcessID = "";
            var Sparator = ";";
            try
            {
                string Query = @"INSERT INTO tbl_process (Name, Requester)
                                VALUES ('" + Name + "', '" + Requester + "')";
                dbAccess.strConn = conn;
                dbAccess.ExecQuery(Query);
                int Count = Approves.Split(new string[] { Sparator }, StringSplitOptions.None).Length;
                for (int i = 0; i < Count; i++)
                {
                    Flag = Flags.Split(new string[] { Sparator }, StringSplitOptions.None)[i];
                    Approve = Approves.Split(new string[] { Sparator }, StringSplitOptions.None)[i];
                    if (Flag == "0" && Approve != "")
                    {
                        AddProcessDetail(ProcessID, Approve);
                    }
                    else if (Flag == "2")
                    {
                        DeleteProcessDetail(ProcessID);
                    }
                }
                result = true;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public bool AddProcessDetail(string ProcessID, string Approve)
        {
            bool result = false;
            var Query = "";
            try
            {
                if (ProcessID == "")
                {
                    Query = @"INSERT INTO tbl_process_detail (ProcessID, UserName)
                                VALUES ((SELECT MAX(ProcessID) FROM tbl_process), '" + Approve + "')";
                }
                else
                {
                    Query = @"INSERT INTO tbl_process_detail (ProcessID, UserName)
                                VALUES ('" + ProcessID + "', '" + Approve + "')";
                }
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                if (res != -1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        public bool EditProcess(string Id, string Name, string Requester, string Flags, string DetailIDs, string Approves)
        {
            bool result = false;
            var Flag = "";
            var DetailID = "";
            var Approve = "";
            var Sparator = ";";
            try
            {
                string Query = @"UPDATE tbl_process SET Name = '" + Name + "', Requester = '" + Requester + "' WHERE ProcessID = '" + Id + "'";
                dbAccess.strConn = conn;
                dbAccess.ExecQuery(Query);
                int Count = Approves.Split(new string[] { Sparator }, StringSplitOptions.None).Length;
                for (int i = 0; i < Count; i++)
                {
                    Flag = Flags.Split(new string[] { Sparator }, StringSplitOptions.None)[i];
                    DetailID = DetailIDs.Split(new string[] { Sparator }, StringSplitOptions.None)[i];
                    Approve = Approves.Split(new string[] { Sparator }, StringSplitOptions.None)[i];
                    if (Flag == "0" && Approve != "")
                    {
                        AddProcessDetail(Id, Approve);
                    }
                    else if (Flag == "1")
                    {
                        EditProcessDetail(DetailID, Approve);
                    }
                    else if (Flag == "2")
                    {
                        DeleteProcessDetail(DetailID);
                    }
                }
                result = true;
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        public bool EditProcessDetail(string Id, string Approve)
        {
            bool result = false;
            try
            {
                string Query = @"UPDATE tbl_process_detail SET UserName = '" + Approve + "' WHERE ProcessDetailID = '" + Id + "'";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                if (res != -1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        public bool DeleteProcess(string Id)
        {
            bool result = false;
            try
            {
                string Query = @"DELETE FROM tbl_process_detail WHERE ProcessID = '" + Id + "' DELETE FROM tbl_process WHERE ProcessID = '" + Id + "' ";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                if (res != -1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        public bool DeleteProcessDetail(string Id)
        {
            bool result = false;
            if (Id == "")
            {
                Id = "0";
            }
            try
            {
                string Query = @"DELETE FROM tbl_process_detail WHERE ProcessDetailID = '" + Id + "'";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                if (res != -1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        //=========================> User <=========================

        public List<StandartComboBox> GetGroups()
        {
            List<StandartComboBox> Result = new List<StandartComboBox>();
            string Query = @"SELECT GroupID as Value, Name  as Text FROM tbl_group ORDER BY Name ASC";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new StandartComboBox
                    {
                        Value = Convert.ToString(dr["Value"]),
                        Text = Convert.ToString(dr["Text"])
                    });
            }
            return Result;

        }

        public List<StandartComboBox> GetDept()
        {
            List<StandartComboBox> Result = new List<StandartComboBox>();
            string Query = @"SELECT IdDept as Value, NamaDept  as Text FROM mDept ORDER BY IdDept ASC";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new StandartComboBox
                    {
                        Value = Convert.ToString(dr["Value"]),
                        Text = Convert.ToString(dr["Text"])
                    });
            }
            return Result;

        }

        public List<StandartComboBox> GetPartner()
        {
            List<StandartComboBox> Result = new List<StandartComboBox>();
            string Query = @"SELECT PartnerID AS Value, CONCAT (PartnerID,' - ', CompanyName) AS Text FROM PartnerDB ORDER BY CompanyName ASC";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new StandartComboBox
                    {
                        Value = Convert.ToString(dr["Value"]),
                        Text = Convert.ToString(dr["Text"])
                    });
            }
            return Result;

        }

        public List<PartnerModel> GetDetailById(string Id)
        {
            List<PartnerModel> Result = new List<PartnerModel>();
            try
            {
                string Query = @"Select * From PartnerDB Where PartnerID = '" + Id + "'";
                DataTable dt = new DataTable();
                dbAccess.strConn = conn;
                dt = dbAccess.GetDataTable(Query);

                foreach (DataRow dr in dt.Rows)
                {
                    Result.Add(
                        new PartnerModel
                        {
                            CompanyName = Convert.ToString(dr["CompanyName"]),
                            Email = Convert.ToString(dr["Email"])
                        });
                }

                return Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<StandartComboBox> GetLevels()
        {
            List<StandartComboBox> Result = new List<StandartComboBox>();
            string Query = @"SELECT LevelID as Value, Name  as Text FROM tbl_level ORDER BY Name ASC";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new StandartComboBox
                    {
                        Value = Convert.ToString(dr["Value"]),
                        Text = Convert.ToString(dr["Text"])
                    });
            }
            return Result;

        }

        public List<ExpandedUserDTO> GetListUsers(string UserName)
        {
            string Query;
            List<ExpandedUserDTO> Result = new List<ExpandedUserDTO>();
            if (UserName == "")
            {
                Query = @"SELECT U.Id, G.NamaDept AS NamaDept, U.NameIdentifier, U.Email, U.UserName FROM AspNetUsers U
                            LEFT OUTER JOIN mDept G ON U.IdDept = G.idDept";
            }
            else
            {
                Query = @"SELECT U.Id, G.NamaDept AS NamaDept, U.NameIdentifier, U.Email, U.UserName FROM AspNetUsers U
                            LEFT OUTER JOIN mDept G ON U.IdDept = G.idDept Where UserName = '" + UserName + "' ORDER BY U.NameIdentifier";
            }
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new ExpandedUserDTO
                    {
                        IdUser = Convert.ToString(dr["Id"]),
                        IdDept = Convert.ToString(dr["NamaDept"]),
                        NameIdentifier = Convert.ToString(dr["NameIdentifier"]),
                        UserName = Convert.ToString(dr["UserName"]),
                        Email = Convert.ToString(dr["Email"])
                    });
            }
            return Result;
        }
        
    }
}