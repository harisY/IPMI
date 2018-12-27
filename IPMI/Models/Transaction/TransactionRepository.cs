using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using IPMI.Models;
using IPMI.Models.Transaction;
using LibDataAccess;

namespace IPMI.Transaction
{
    public class TransactionRepository
    {
        MyLib dbAccess = new MyLib();
        string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // ===========================> Rrequest <===========================
        public List<StandartComboBox> GetProcess(string UserLogin)
        {
            List<StandartComboBox> Result = new List<StandartComboBox>();
            string Query = @"SELECT ProcessID as Value, Name  as Text FROM tbl_process WHERE Requester = '" + UserLogin + "' ORDER BY Name ASC";
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

        public List<TransactionRequest> GetListRequest(string UserLogin)
        {
            string Query = "";
            List<TransactionRequest> Result = new List<TransactionRequest>();
            string sql = @"Select UR.RoleId from AspNetUserRoles as UR 
                            left outer join AspNetRoles as R on UR.RoleId = R.Id
                            left outer join AspNetUsers as U on UR.UserId = U.Id
                            where U.UserName = '" + UserLogin + "' and R.Name = 'Administrator'";
            DataTable data = new DataTable();
            dbAccess.strConn = conn;
            data = dbAccess.GetDataTable(sql);
            if (data.Rows.Count == 0)
            {
                Query = @"SELECT R.RequestID, R.RequestDate, R.ProcessID, P.Name AS ProcessName, R.NoReff, R.NoCash, R.Status, R.TotalRequest, R.UserName, U.NameIdentifier AS Maker
                             FROM tbl_request R LEFT OUTER JOIN tbl_process P ON R.ProcessID = P.ProcessID
                             LEFT OUTER JOIN AspNetUsers U ON R.UserName = U.UserName
							 WHERE R.UserName = '" + UserLogin + "' ORDER BY RequestID DESC";
            }
            else
            {
                Query = @"SELECT R.RequestID, R.RequestDate, R.ProcessID, P.Name AS ProcessName, R.NoReff, R.NoCash, R.Status, R.TotalRequest, R.UserName, U.NameIdentifier AS Maker
                             FROM tbl_request R LEFT OUTER JOIN tbl_process P ON R.ProcessID = P.ProcessID
                             LEFT OUTER JOIN AspNetUsers U ON R.UserName = U.UserName ORDER BY RequestID DESC";
            }
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new TransactionRequest
                    {
                        RequestID = Convert.ToString(dr["RequestID"]),
                        RequestDate = Convert.ToString(dr["RequestDate"]),
                        ProcessID = Convert.ToString(dr["ProcessID"]),
                        ProcessName = Convert.ToString(dr["ProcessName"]),
                        NoReff = Convert.ToString(dr["NoReff"]),
                        NoCash = Convert.ToString(dr["NoCash"]),
                        TotalRequest = Convert.ToString(dr["TotalRequest"]),
                        Status = Convert.ToString(dr["Status"]),
                        Maker = Convert.ToString(dr["Maker"])
                    });
            }
            return Result;
        }

        public List<TransactionRequest> GetListRequestDetail(string id)
        {
            List<TransactionRequest> Result = new List<TransactionRequest>();
            string Query = @"SELECT * FROM tbl_request_detail WHERE RequestID = '" + id + "' ORDER BY RequestDetailID ASC";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new TransactionRequest
                    {
                        RequestDetailID = Convert.ToString(dr["RequestDetailID"]),
                        Description = Convert.ToString(dr["Description"]),
                        Amount = Convert.ToString(dr["Amount"])
                    });
            }
            return Result;
        }

        public List<TransactionRequest> GetListRequestHistory(string id)
        {
            List<TransactionRequest> Result = new List<TransactionRequest>();
            string Query = @"SELECT L.Name AS LevelName, U.NameIdentifier, RA.RequestActionDate, RA.ActionID, RA.Comment FROM tbl_request_action RA
                            LEFT OUTER JOIN AspNetUsers U ON RA.UserName = U.UserName
                            LEFT OUTER JOIN tbl_level L ON U.IdLevel = L.LevelID
                            WHERE RA.RequestID = '" + id + "' ORDER BY RequestID ASC";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new TransactionRequest
                    {
                        LevelName = Convert.ToString(dr["LevelName"]),
                        Approve = Convert.ToString(dr["NameIdentifier"]),
                        ActionDate = Convert.ToString(dr["RequestActionDate"]),
                        ActionID = Convert.ToString(dr["ActionID"]),
                        Comment = Convert.ToString(dr["Comment"])
                    });
            }
            return Result;
        }

        public bool AddRequest(string Process, string NoReff, string NoCash, string TotalAmount, string Flags, string Descriptions, string Amounts, string UserLogin)
        {
            bool result = false;
            var Flag = "";
            var Description = "";
            var Amount = "";
            var RequestID = "";
            var Sparator = ";";
            try
            {
                string Query = @"INSERT INTO tbl_request (ProcessID, RequestDate, UserName, NoReff, NoCash, Status, TotalRequest)
                                VALUES ('" + Process + "', SYSDATETIME(), '" + UserLogin + "', '" + NoReff + "', '" + NoCash + "', 'Draff', '" + TotalAmount + "')";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
            int Count = Amounts.Split(new string[] { Sparator }, StringSplitOptions.None).Length;
                for (int i = 0; i < Count; i++)
                {
                    Flag = Flags.Split(new string[] { Sparator }, StringSplitOptions.None)[i];
                    Description = Descriptions.Split(new string[] { Sparator }, StringSplitOptions.None)[i];
                    Amount = Amounts.Split(new string[] { Sparator }, StringSplitOptions.None)[i];
                    if (Flag == "0" && Amount != "")
                    {
                        AddRequestDetail(RequestID, Description, Amount);
                    }
                    else if (Flag == "2")
                    {
                        DeleteRequestDetail(RequestID);
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

        public bool AddRequestDetail(string RequestID, string Description, string Amount)
        {
            bool result = false;
            var Query = "";
            try
            {
                if (RequestID == "")
                {
                    Query = @"INSERT INTO tbl_request_detail (RequestID, Description, Amount)
                                VALUES ((SELECT MAX(RequestID) FROM tbl_request), '" + Description + "', '" + Amount + "')";
                }
                else
                {
                    Query = @"INSERT INTO tbl_request_detail (RequestID, Description, Amount)
                                VALUES ('" + RequestID + "', '" + Description + "', '" + Amount + "')";
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

        public bool EditRequest(string Id, string Process, string NoReff, string NoCash, string TotalAmount, string Flags, string DetailIDs, string Descriptions, string Amounts, string UserLogin)
        {
            bool result = false;
            var Flag = "";
            var DetailID = "";
            var Description = "";
            var Amount = "";
            var Sparator = ";";
            try
            {
                string Query = @"UPDATE tbl_request SET ProcessID = '" + Process + "', RequestDate = SYSDATETIME(), UserName = '" + UserLogin + "', NoReff = '" + NoReff + "', NoCash = '" + NoCash + "', TotalRequest = '" + TotalAmount + "'   WHERE RequestID = '" + Id + "'";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                int Count = Amounts.Split(new string[] { Sparator }, StringSplitOptions.None).Length;
                for (int i = 0; i < Count; i++)
                {
                    Flag = Flags.Split(new string[] { Sparator }, StringSplitOptions.None)[i];
                    DetailID = DetailIDs.Split(new string[] { Sparator }, StringSplitOptions.None)[i];
                    Description = Descriptions.Split(new string[] { Sparator }, StringSplitOptions.None)[i];
                    Amount = Amounts.Split(new string[] { Sparator }, StringSplitOptions.None)[i];
                    if (Flag == "0" && Amount != "")
                    {
                        AddRequestDetail(Id, Description, Amount);
                    }
                    else if (Flag == "1")
                    {
                        EditRequestDetail(DetailID, Description, Amount);
                    }
                    else if (Flag == "2")
                    {
                        DeleteRequestDetail(DetailID);
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

        public bool EditRequestDetail(string Id, string Description, string Amount)
        {
            bool result = false;
            try
            {
                string Query = @"UPDATE tbl_request_detail SET Description = '" + Description + "', Amount = '" + Amount + "' WHERE RequestDetailID = '" + Id + "'";
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

        public bool DeleteRequestDetail(string Id)
        {
            bool result = false;
            if (Id == "")
            {
                Id = "0";
            }
            try
            {
                string Query = @"DELETE FROM tbl_request_detail WHERE RequestDetailID = '" + Id + "'";
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

        public bool DeleteRequest(string Id)
        {
            bool result = false;
            try
            {
                string Query = @"DELETE FROM tbl_request_detail WHERE RequestID = '" + Id + "' DELETE FROM tbl_request WHERE RequestID = '" + Id + "' ";
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

        public bool SubmitRequest(string Id, string Process)
        {
            bool result = false;
            string Query = @"SELECT PD.UserName AS ID from tbl_process_detail PD
                            LEFT OUTER JOIN	AspNetUsers U ON PD.UserName = U.UserName
                            LEFT OUTER JOIN tbl_level L ON U.IdLevel = L.LevelID
                            where PD.ProcessID = '" + Process + "' and L.LevelID = '2'";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);
            foreach (DataRow dr in dt.Rows)
            {
                var UserID = Convert.ToString(dr["ID"]);
                try
                {
                    string sql = @"INSERT INTO tbl_request_action (RequestID, ActionID, IsActive, IsComplete, UserName)
                                VALUES ('" + Id + "', '0', '1','0','" + UserID + "') UPDATE tbl_request SET Status = 'On process'   WHERE RequestID = '" + Id + "'";
                    dbAccess.strConn = conn;
                    int res = dbAccess.ExecQuery(sql);
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
            }
            return result;
        }

        // ===========================> Task <===========================

        public List<TransactionRequest> GetListTask(string UserLogin)
        {
            string Query = "";
            List<TransactionRequest> Result = new List<TransactionRequest>();
            string sql = @"Select UR.RoleId from AspNetUserRoles as UR 
                            left outer join AspNetRoles as R on UR.RoleId = R.Id
                            left outer join AspNetUsers as U on UR.UserId = U.Id
                            where U.UserName = '" + UserLogin + "' and R.Name = 'Administrator'";
            DataTable data = new DataTable();
            dbAccess.strConn = conn;
            data = dbAccess.GetDataTable(sql);
            if (data.Rows.Count == 0)
            {
                Query = @"SELECT RA.RequestActionID, U2.IdLevel, R.RequestID, R.RequestDate, R.ProcessID, P.Name AS ProcessName, R.NoReff, R.NoCash, R.Status, R.TotalRequest, R.UserName, U.NameIdentifier AS Maker
                             FROM tbl_request_action RA LEFT OUTER JOIN tbl_request R ON RA.RequestID = R.RequestID
							 LEFT OUTER JOIN tbl_process P ON R.ProcessID = P.ProcessID
                             LEFT OUTER JOIN AspNetUsers U ON R.UserName = U.UserName
                             LEFT OUTER JOIN AspNetUsers U2 ON RA.UserName = U2.UserName
							 WHERE RA.UserName = '" + UserLogin + "' AND RA.ActionID = '0' AND RA.IsActive = '1' AND RA.IsComplete ='0' ORDER BY RequestID DESC";
            }
            else
            {
                Query = @"SELECT RA.RequestActionID, U2.IdLevel, R.RequestID, R.RequestDate, R.ProcessID, P.Name AS ProcessName, R.NoReff, R.NoCash, R.Status, R.TotalRequest, R.UserName, U.NameIdentifier AS Maker
                             FROM tbl_request_action RA LEFT OUTER JOIN tbl_request R ON RA.RequestID = R.RequestID
							 LEFT OUTER JOIN tbl_process P ON R.ProcessID = P.ProcessID
                             LEFT OUTER JOIN AspNetUsers U ON R.UserName = U.UserName
                             LEFT OUTER JOIN AspNetUsers U2 ON RA.UserName = U2.UserName
							 WHERE RA.ActionID = '0' AND RA.IsActive = '1' AND RA.IsComplete ='0' ORDER BY RequestID DESC";
            }
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new TransactionRequest
                    {
                        RequestActionID = Convert.ToString(dr["RequestActionID"]),
                        LevelID = Convert.ToString(dr["IdLevel"]),
                        RequestID = Convert.ToString(dr["RequestID"]),
                        RequestDate = Convert.ToString(dr["RequestDate"]),
                        ProcessID = Convert.ToString(dr["ProcessID"]),
                        ProcessName = Convert.ToString(dr["ProcessName"]),
                        NoReff = Convert.ToString(dr["NoReff"]),
                        NoCash = Convert.ToString(dr["NoCash"]),
                        TotalRequest = Convert.ToString(dr["TotalRequest"]),
                        Status = Convert.ToString(dr["Status"]),
                        Maker = Convert.ToString(dr["Maker"])
                    });
            }
            return Result;
        }

        public List<TransactionRequest> GetListTaskDetail(string id)
        {
            List<TransactionRequest> Result = new List<TransactionRequest>();
            string Query = @"SELECT * FROM tbl_request_detail WHERE RequestID = '" + id + "' ORDER BY RequestDetailID ASC";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new TransactionRequest
                    {
                        RequestDetailID = Convert.ToString(dr["RequestDetailID"]),
                        Description = Convert.ToString(dr["Description"]),
                        Amount = Convert.ToString(dr["Amount"])
                    });
            }
            return Result;
        }

        public List<TransactionRequest> GetListTaskHistory(string id)
        {
            List<TransactionRequest> Result = new List<TransactionRequest>();
            string Query = @"SELECT L.Name AS LevelName, U.NameIdentifier, RA.RequestActionDate, RA.ActionID FROM tbl_request_action RA
                            LEFT OUTER JOIN AspNetUsers U ON RA.UserName = U.UserName
                            LEFT OUTER JOIN tbl_level L ON U.IdLevel = L.LevelID
                            WHERE RA.RequestID = '" + id + "' ORDER BY RequestID ASC";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new TransactionRequest
                    {
                        LevelName = Convert.ToString(dr["LevelName"]),
                        Approve = Convert.ToString(dr["NameIdentifier"]),
                        ActionDate = Convert.ToString(dr["RequestActionDate"]),
                        ActionID = Convert.ToString(dr["ActionID"])
                    });
            }
            return Result;
        }

        public bool Approve(string Id, string IdTask, string IdLevel, string Process)
        {
            bool result = false;
            try
            {
                string Query = @"UPDATE tbl_request_action SET ActionID = '1', RequestActionDate = SYSDATETIME(), IsActive = '0', IsComplete = '1' WHERE RequestActionID = '" + IdTask + "'";
                dbAccess.strConn = conn;
                int res = dbAccess.ExecQuery(Query);
                if (IdLevel == "2")
                {
                    string Query2 = @"SELECT * FROM tbl_request_action RA 
                    LEFT OUTER JOIN AspNetUsers U ON RA.UserName = U.UserName
		            WHERE RA.RequestID = '" + Id + "' AND RA.ActionID = '0' AND RA.IsActive = '1' AND RA.IsComplete ='0' AND U.IdLevel = '2'";
                    DataTable dt = new DataTable();
                    dbAccess.strConn = conn;
                    dt = dbAccess.GetDataTable(Query2);
                    if (dt.Rows.Count == 0)
                    {
                        string Query3 = @"SELECT PD.UserName AS ID from tbl_process_detail PD
                        LEFT OUTER JOIN	AspNetUsers U ON PD.UserName = U.UserName
                        LEFT OUTER JOIN tbl_level L ON U.IdLevel = L.LevelID
                        where PD.ProcessID = '" + Process + "' and L.LevelID = '3'";
                        DataTable dt3 = new DataTable();
                        dbAccess.strConn = conn;
                        dt3 = dbAccess.GetDataTable(Query3);
                        foreach (DataRow dr in dt3.Rows)
                        {
                            var UserID = Convert.ToString(dr["ID"]);
                            string sql = @"INSERT INTO tbl_request_action (RequestID, ActionID, IsActive, IsComplete, UserName)
                                VALUES ('" + Id + "', '0', '1','0','" + UserID + "')";
                            dbAccess.strConn = conn;
                            dbAccess.ExecQuery(sql);
                        }
                    }
                    result = true;
                }
                if (IdLevel == "3")
                {
                    string Query4 = @"UPDATE tbl_request SET Status = 'Approve'   WHERE RequestID = '" + Id + "'";
                    dbAccess.strConn = conn;
                    int res2 = dbAccess.ExecQuery(Query4);
                    if (res2 != -1)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        public bool Reject(string Id, string IdTask, string Comment)
        {
            bool result = false;
            try
            {
                string Query = @"UPDATE tbl_request_action SET ActionID = '3', RequestActionDate = SYSDATETIME(), IsActive = '0', IsComplete = '1', Comment ='" + Comment + "' WHERE RequestActionID = '" + IdTask + "' UPDATE tbl_request_action SET IsActive = '0', IsComplete = '1' WHERE RequestID = '" + Id + "' UPDATE tbl_request SET Status = 'Reject' WHERE RequestID = '" + Id + "'";
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
    }
}