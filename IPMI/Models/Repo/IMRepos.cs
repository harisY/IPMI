using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using LibDataAccess;
using IPMI.Models.IM;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace IPMI.Models.Repo
{
    public class IMRepos
    {
        MyLib dbAccess = new MyLib();
        
        string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private CultureInfo id = new CultureInfo("id-ID");
        public List<IMModels> getListIM(string UserName)
        {
            List<IMModels> iMs = new List<IMModels>();
            string query =
                @"SELECT
                     a.[NoIPMI]
                    ,convert(date,a.[Tgl]) Tgl
                    ,a.[Dari]
                    ,b.NamaDept
                    ,a.[Masalah]
                    ,a.FileName
                    ,a.[Jumlah]
                    ,a.[Status]
                FROM [tIpmi] a INNER JOIN 
                mDept b on a.dari = b.IdDept ORDER By a.NoIPMI, a.Dari ASC";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(query);

            foreach (DataRow row in dt.Rows)
            {
                DateTime tgl = Convert.ToDateTime(row["Tgl"]);
                string idTgl = tgl.ToString("dd/MM/yyyy", id);
                iMs.Add(
                    new IMModels
                    {
                        NoIPMI = Convert.ToString(row["NoIPMI"]),
                        Tgl = idTgl,
                        Dari = Convert.ToString(row["Dari"]),
                        Masalah = Convert.ToString(row["Masalah"]),
                        FileName = Convert.ToString(row["FileName"]),
                        Jumlah = (int)(row["Jumlah"]),
                        Status = Convert.ToString(row["Status"]),
                        NamaDept = Convert.ToString(row["NamaDept"])
                    });
            }
            return iMs;
        }

        public List<IMModels> getIMByID(string NoIPMI)
        {
            List<IMModels> Ims = new List<IMModels>();
            string query =
                @"SELECT
                     [NoIPMI]
                    ,[Tgl]
                    ,[Dari]
                    ,[ke]
                    ,[Masalah]
                    ,[Jumlah]
                    ,[Status] 
                FROM [tIpmi] 
                WHERE NoIPMI = '" + NoIPMI + "'";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(query);

            foreach (DataRow row in dt.Rows)
            {
                DateTime tgl = Convert.ToDateTime(row["Tgl"]);
                string idTgl = tgl.ToString("dd/MM/yyyy", id);
                Ims.Add(
                    new IMModels
                    {
                        NoIPMI = Convert.ToString(row["IdDept"]),
                        Tgl = idTgl,
                        Dari = Convert.ToString(row["Dari"]),
                        Ke = Convert.ToString(row["Ke"]),
                        Masalah = Convert.ToString(row["Masalah"]),
                        Jumlah = (int)(row["Jumlah"]),
                        Status = Convert.ToString(row["Status"])
                    });
            }
            return Ims;
        }

        public DataTable GetDeptByUserName(string UserName)
        {
            try
            {
                string query =
                @"SELECT
                     a.IdDept IdDept, b.NamaDept NamaDept 
                FROM [AspNetUsers] a INNER JOIN
                    mDept b on a.IdDept = b.IdDept 
                WHERE UserName = '" + UserName + "'";
                DataTable dt = new DataTable();
                dbAccess.strConn = conn;
                dt = dbAccess.GetDataTable(query);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void AddIM(IMModels Ims)
        {
            try
            {
                string query =
                    @"INSERT INTO [tIpmi]
                        ([NoIPMI]
                        ,[Tgl]
                        ,[Dari]
                        ,[Masalah]
                        ,[Jumlah]
                        ,[Status]
                        ,[FileName]
                        ,[CreatedBy]
                        ,[CreatedDate])
                    VALUES
                        ('" + Ims.NoIPMI + "'" +
                        ",'" + DateTime.Parse(Ims.Tgl) + "'" +
                        ",'" + Ims.Dari + "'" +
                        ",'" + Ims.Masalah + "'" +
                        ",'" + Ims.Jumlah + "'" +
                        ",'Open' " +
                        ",'" + Ims.FileName + "'" +
                        ",'" + Ims.CreatedBy + "'" +
                        ",GETDATE())";
                dbAccess.strConn = conn;
                dbAccess.ExecQuery(query);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void AddIMDetails(IMDetailsModels iMDetails)
        {
            try
            {
                string query =
                    @"INSERT INTO [tIPMIDetail]
                        ([NoIPMI]
                        ,[Ke]
                        ,[Status])
                    VALUES
                        ('" + iMDetails.NoIPMI + "'" +
                        ",'" + iMDetails.Ke + "'" +
                        ",'Open')";
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
        public void EditIM(IMModels iM)
        {
            try
            {
                string query =
                    @"UPDATE [tIpmi]
                    SET 
                         [Tgl] = '" + DateTime.Parse(iM.Tgl) + "'" +
                        ",[Dari] = '" + iM.Dari + "'" +
                        ",[ke] = '" + iM.Ke + "'" +
                        ",[Masalah] = '" + iM.Masalah + "'" +
                        ",[Jumlah] ='" + iM.Jumlah + "'" +
                        ",[Status] ='" + iM.Status + "'" +
                        ",[FileName] ='" + iM.FileName + "'" +
                        ",[UpdatedBy] = '" + iM.UpdatedBy + "'" +
                        ",[UpdateDate] = GETDATE() " +
                    "WHERE NoIPMI = '" + iM.NoIPMI + "'";
                dbAccess.strConn = conn;
                dbAccess.ExecQuery(query);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void EditIMDetails(IMDetailsModels iMDetails)
        {
            try
            {
                string query =
                    @"UPDATE [tipmidetail]
                    SET 
                         [Ke] = '" + iMDetails.Ke + "'" +
                    "WHERE NoIPMI = '" + iMDetails.NoIPMI + "'";
                dbAccess.strConn = conn;
                dbAccess.ExecQuery(query);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteIMById(string NoIPMI)
        {
            try
            {
                string query = string.Format("" +
                    "DELETE FROM tIpmi WHERE NoIPMI = '" + NoIPMI + "'");
                dbAccess.strConn = conn;
                dbAccess.ExecQuery(query);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void DeleteIMDetailById(string NoIPMI)
        {
            try
            {
                string query = string.Format("" +
                    "DELETE FROM tipmidetail WHERE NoIPMI = '" + NoIPMI + "'");
                dbAccess.strConn = conn;
                dbAccess.ExecQuery(query);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string AutoNo(String IdDept)
        {
            try
            {
                string sql =
                    @"DECLARE @Tahun varchar(4)
                            , @Bulan varchar(2) 
                            , @seq varchar(4)
                            , @ipmi char(4)
                            , @dept varchar(4)
                    SET @ipmi ='IPMI'
                    SET @Tahun = datepart(year,getdate()) 
                    SET @Bulan = SUBSTRING(CONVERT(nvarchar(6),getdate(), 112),5,2) 
                    SET @dept = '" + IdDept + "' " +
                    @"SET @seq = (SELECT RIGHT('0000'+ CAST(SUBSTRING(RTRIM(MAX(noipmi)),9,4) + 1 as Varchar),4) FROM tIpmi WHERE dari = @dept)
                    SELECT @ipmi + @dept + COALESCE(@seq, '0001') + @Bulan + @Tahun as AutoNo ";

                DataTable dt = new DataTable();
                dbAccess.strConn = conn;
                dt = dbAccess.GetDataTable(sql);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IMModels> GetListRequestDetail(string id)
        {
            List<IMModels> Result = new List<IMModels>();
            string Query = @"SELECT b.NamaDept NamaDept FROM tIPMIDetail a INNER JOIN mDept b on a.ke = b.IdDept   
                            WHERE a.NoIPMI = '" + id + "' ORDER BY NoIPMI ASC";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(Query);

            foreach (DataRow dr in dt.Rows)
            {
                Result.Add(
                    new IMModels
                    {
                        Ke = Convert.ToString(dr["NamaDept"]),
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
        public List<StandartComboBox> GetDeptByUserLogin(string UserName)
        {
            List<StandartComboBox> Result = new List<StandartComboBox>();
            string query =
                @"SELECT
                     IdDept Value, NamaDept Text  
                FROM mDept 
                WHERE IdDept <>(Select IdDept from [AspNetUsers] where Username = '" + UserName + "')" +
                "Order by NamaDept";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(query);

            Result.Add(
                new StandartComboBox
                {
                    Text = "-- SELECT DEPARTMENT --",
                    Value = "0"
                });
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
        public Collection<IMDetailsModels> DetailsModels { get; set; } = new Collection<IMDetailsModels>();

        public void InsertIM(IMModels models)
        {
            try
            {
                using (SqlConnection Conn1 = new SqlConnection(conn))
                {
                    Conn1.Open();
                    using (SqlTransaction Trans1 = Conn1.BeginTransaction())
                    {
                        dbAccess.gh_Trans = new LibDataAccess.TransactionHelper();
                        dbAccess.gh_Trans.Command.Connection = Conn1;
                        dbAccess.gh_Trans.Command.Transaction = Trans1;

                        try
                        {

                            AddIM(models);
                            IMDetailsModels detailsModels = new IMDetailsModels();
                            for (int i = 0; i <= DetailsModels.Count - 1; i++)
                            {
                                {
                                    AddIMDetails(detailsModels);
                                    //detailsModels.NoIPMI = DetailsModels.[i].ToString();
                                }
                            }

                            Trans1.Commit();
                        }
                        catch (Exception)
                        {
                            Trans1.Rollback();
                            throw;
                        }
                        finally
                        {
                            dbAccess.gh_Trans = null;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}