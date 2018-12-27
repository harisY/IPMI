using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using LibDataAccess;
using IPMI.Models.Master;
using System.Data;
using System.Web.WebPages.Html;

namespace IPMI.Models.Repo
{
    public class deptRepos
    {
        MyLib dbAccess = new MyLib();
        string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<deptModels> getListDepts()
        {
            List<deptModels> depts = new List<deptModels>();
            string query = string.Format("" +
                "SELECT IdDEpt," +
                        "NamaDept " +
                "FROM mDept " +
                "ORDER BY NamaDept");
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(query);

            foreach (DataRow row in dt.Rows)
            {
                depts.Add(
                    new deptModels
                    {
                        IdDept = Convert.ToString(row["IdDept"]),
                        NamaDept = Convert.ToString(row["NamaDept"])
                    });
            }
            return depts;
        }

        public string getDeptByUser(string name)
        {
            string query = @"SELECT IdDept FROM AspNetUsers WHERE UserName ='" + name + "'";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(query);

            return dt.Rows[0][0].ToString();
        }

        public List<deptModels> getDeptByID(string IdDept)
        {
            List<deptModels> depts = new List<deptModels>();
            string query = string.Format("" +
                "SELECT IdDEpt," +
                        "NamaDept " +
                "FROM mDept " +
                "WHERE IdDept= '" + IdDept + "'");
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(query);

            foreach (DataRow row in dt.Rows)
            {
                depts.Add(
                    new deptModels
                    {
                        IdDept = Convert.ToString(row["IdDept"]),
                        NamaDept = Convert.ToString(row["NamaDept"])
                    });
            }
            return depts;
        }
        public void AddDept(deptModels dept)
        {
            try
            {
                string query = string.Format("" +
                    "INSERT INTO mDept(IdDept,NamaDept)" +
                    "VALUES('" + dept.IdDept + "'," +
                            "'" + dept.NamaDept + "')");
                dbAccess.strConn = conn;
                dbAccess.ExecQuery(query);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool isDeptExist(string IdDept)
        {
            try
            {
                string query = string.Format("" +
                    "SELECT IdDept " +
                    "FROM mDept " +
                    "WHERE IdDept = '" + IdDept + "'");
                DataTable dt = new DataTable();
                dbAccess.strConn = conn;
                dt = dbAccess.GetDataTable(query);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void EditDept(deptModels dept)
        {
            try
            {
                string query = string.Format("" +
                    "UPDATE mDept " +
                    "SET NamaDept = '" + dept.NamaDept + "'" +
                    "WHERE IdDept = '" + dept.IdDept + "'");
                dbAccess.strConn = conn;
                dbAccess.ExecQuery(query);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool IsDataExist(string Dept)
        {
            bool IsExist;
            try
            {
                string sql =
                    @"SELECT IdDept from [AspNetUsers] where IdDept='" + Dept + "'";
                DataTable dt = new DataTable();
                dbAccess.strConn = conn;
                dt = dbAccess.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    IsExist = true;
                }
                else
                {
                    IsExist = false;
                }
                return IsExist;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void DeleteDeptById(string IdDept)
        {
            try
            {
                string query = string.Format("" +
                    "DELETE FROM mDept WHERE IdDept = '" + IdDept + "'");
                dbAccess.strConn = conn;
                dbAccess.ExecQuery(query);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}