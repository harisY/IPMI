using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IPMI.Entity;
using IPMI.Models;
using IPMI.Repository;
using LibDataAccess;
namespace IPMI.Services
{
    public partial class IMService
    {
        private GenericRepository<tIpmi> IMRepository;
        MyLib myLib = new MyLib();
        string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        string connSolomon = ConfigurationManager.ConnectionStrings["SolomonConnection"].ConnectionString;
        public IMService()
        {
            this.IMRepository = new GenericRepository<tIpmi>(new IPMI_DBEntities());
        }

        public IEnumerable<tIpmi> GetAll()
        {
            string spQuery = "[Get_IPMI_Global]";
            return IMRepository.ExecuteQuery(spQuery);
        }
        public IEnumerable<tIpmi> GetAll(object[] parameters)
        {
            string spQuery = "[Get_IPMI] {0}";
            return IMRepository.ExecuteQuery(spQuery, parameters);
        }
        public IEnumerable<tIpmi> GetIpmiClosed(object[] parameters)
        {
            string spQuery = "[Get_IPMI_Closed] {0}";
            return IMRepository.ExecuteQuery(spQuery, parameters);
        }
        public IEnumerable<tIpmi> Get_IPMI_Verifikasi(object[] parameters)
        {
            string spQuery = "[Get_IPMI_Verifikasi] {0}";
            return IMRepository.ExecuteQuery(spQuery, parameters);
        }
        public tIpmi GetbyID(object[] parameters)
        {
            string spQuery = "[Get_IpmibyID] {0}";
            return IMRepository.ExecuteQuerySingle(spQuery, parameters);
        }

        public int Insert(object[] parameters)
        {
            string spQuery = "[Set_Ipmi] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7},{8},{9},{10}";
            return IMRepository.ExecuteCommand(spQuery, parameters);
        }

        public int Update(object[] parameters)
        {           
            string spQuery = "[Update_Ipmi]{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8},{9},{10}";
            return IMRepository.ExecuteCommand(spQuery, parameters);
        }
        public int UpdateStatusIpmi(object[] parameters)
        {
            string spQuery = "[Update_Status_Ipmi]{0}";
            return IMRepository.ExecuteCommand(spQuery, parameters);
        }
        public int Update_Status_Ipmi_Closed(object[] parameters)
        {
            string spQuery = "[Update_Status_Ipmi_Closed]{0}";
            return IMRepository.ExecuteCommand(spQuery, parameters);
        }
        public int SetHistoryIpmi_UpdateStatus1(object[] parameters)
        {
            string spQuery = "[SetHistory_UpdateStatus1]{0},{1},{2}";
            return IMRepository.ExecuteCommand(spQuery, parameters);
        }
        public int UpdateStatusIpmi_Open(object[] parameters)
        {
            string spQuery = "[Update_Status_Ipmi_Open]{0}";
            return IMRepository.ExecuteCommand(spQuery, parameters);
        }
        public int Delete(object[] parameters)
        {
            string spQuery = "[Delete_Ipmi] {0}";
            return IMRepository.ExecuteCommand(spQuery, parameters);
        }
        public string AutoNo(object[] parameters)
        {
            string spQuery = "[Get_AutoNo] {0}";
            return IMRepository.ExecuteQuerySingle1(spQuery, parameters);
        }

        public IEnumerable<tIpmi> ReportGetAll(object[] parameters)
        {
            string spQuery = "[Report_GetAll] {0}";
            return IMRepository.ExecuteQuery(spQuery, parameters);
        }

        public List<FileModels> GetFiles()
        {
            List<FileModels> files = new List<FileModels>();
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM tFile"))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            files.Add(new FileModels
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                Name = sdr["Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return files;
        }

        public string GetEmail(string Ke)
        {
            string Email = string.Empty;
            try
            {
                string que = @"SELECT top 1 Email FROM AspNetUsers where IdDept='" + Ke + "'";
                DataTable dt = new DataTable();
                myLib.strConn = conn;
                dt = myLib.GetDataTable(que);
                if (dt.Rows.Count > 0)
                {
                    Email = dt.Rows[0][0].ToString();
                }
                return Email;
            }
            catch (Exception)
            {

                throw;
            }
            

        }

        public string GetEmailAnalisa(string Dari)
        {
            string Email = string.Empty;
            try
            {
                string que = @"SELECT top 1 Email FROM AspNetUsers where IdDept='" + Dari + "'";
                DataTable dt = new DataTable();
                myLib.strConn = conn;
                dt = myLib.GetDataTable(que);
                if (dt.Rows.Count > 0)
                {
                    Email = dt.Rows[0][0].ToString();
                }
               
                
                return Email;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public DataTable GetIpmiByNo(string NoIpmi)
        {
            try
            {
                string que = @"SELECT Dari, Ke, Masalah From tIpmi where NoIPMI = '" + NoIpmi + "'";
                DataTable dt = new DataTable();
                myLib.strConn = conn;
                dt = myLib.GetDataTable(que);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataTable GetCustomer()
        {
            try
            {
                string que = @"select CustId, Name from Customer";
                DataTable dt = new DataTable();
                myLib.strConn = connSolomon;
                dt = myLib.GetDataTable(que);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}