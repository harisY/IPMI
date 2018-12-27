using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using LibDataAccess;
using IPMI.Models.Analisa;
using IPMI.Models.IM;
using System.Data;
using System.Web.WebPages.Html;
using System.Globalization;
using System.Security.Principal;

namespace IPMI.Models.Repo
{
    public class AnalisaRepos
    {
        MyLib dbAccess = new MyLib();
        string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private CultureInfo id = new CultureInfo("id-ID");
        public List<IMModels> getListAnalisa(string Dept)
        {
            List<AnalisaModels> analisas = new List<AnalisaModels>();
            List<IMModels> models = new List<IMModels>();
            string query =
            //@"SELECT a.[id]
            //,b.[NoIPMI]
            //,CONVERT(date,b.[Tgl]) Tgl
            //,b.[Dari]
            //,b.Ke
            //,b.Masalah
            //,a.[Penyebab]
            //,a.[RencanaPerbaikan]
            //,a.[Target]
            //,a.[PIC]
            //,a.[TglActual]
            //,b.[Status]
            //,a.[InputByDept]
            //FROM [IPMI_DB].[dbo].[tIpmiAnalisa] a RIGHT JOIN
            //(SELECT a.NoIPMI,a.Tgl,a.Dari,b.Ke,a.Masalah,a.Jumlah,a.Status
            //FROM tIpmi a inner join
            //tipmidetail b ON a.NoIpmi = b.NoIpmi
            //WHERE b.Ke = '" + Dept + "' and a.[Status]='Open') b on a.NoIPMI = b.NoIPMI";
            @"SELECT a.NoIPMI,a.Tgl,a.Dari,a.Masalah,a.Jumlah,a.Status, b.status [Status1], a.FileName
            FROM tIpmi a inner join
                tipmidetail b ON a.NoIpmi = b.NoIpmi
            WHERE b.Ke = '" + Dept + "'";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(query);


            foreach (DataRow row in dt.Rows)
            {
                DateTime tgl = Convert.ToDateTime(row["Tgl"]);
                string idTgl = tgl.ToString("dd/MM/yyyy", id);
                //analisas.Add(
                //    new AnalisaModels
                //    {
                //        NoIPMI = Convert.ToString(row["NoIPMI"]),
                //        Tgl = idTgl,
                //        Dari = Convert.ToString(row["Dari"]),
                //        Masalah = Convert.ToString(row["Masalah"]),
                //        Penyebab = Convert.ToString(row["Penyebab"]),
                //        RPerbaikan = Convert.ToString(row["RencanaPerbaikan"]),
                //        Target = Convert.ToString(row["Target"]),
                //        PIC = Convert.ToString(row["PIC"]),
                //        TglActual = Convert.ToString(row["TglActual"]),
                //        Status = Convert.ToString(row["Status"]),
                //        InputByDept = Convert.ToString(row["InputByDept"])
                //    });
                models.Add(new IMModels {
                    NoIPMI = Convert.ToString(row["NoIPMI"]),
                    Tgl = idTgl,
                    Dari = Convert.ToString(row["Dari"]),
                    Masalah = Convert.ToString(row["Masalah"]),
                    Jumlah = (int)(row["Jumlah"]),
                    Status = Convert.ToString(row["Status"]),
                    FileName = Convert.ToString(row["FileName"])
                });


            }
            return models;
        }
        public List<AnalisaModels> getListAnalisaByNoIpmi(string NoIpmi, string dept)
        {
            List<AnalisaModels> analisas = new List<AnalisaModels>();
            string query =
            @"SELECT [id]
            ,[NoIPMI]
            ,[Penyebab]
            ,[RencanaPerbaikan]
            ,[Target]
            ,[PIC]
            ,CONVERT(date,[TglActual]) Tgl
            FROM [tIpmiAnalisa] WHERE NoIPMI='" + NoIpmi + "' and inputByDept = '" + dept + "'";
            DataTable dt = new DataTable();
            dbAccess.strConn = conn;
            dt = dbAccess.GetDataTable(query);


            foreach (DataRow row in dt.Rows)
            {
                DateTime tgl = Convert.ToDateTime(row["Tgl"]);
                string idTgl = tgl.ToString("dd/MM/yyyy", id);
                analisas.Add(
                    new AnalisaModels
                    {
                        NoIPMI = Convert.ToString(row["NoIPMI"]),
                        Penyebab = Convert.ToString(row["Penyebab"]),
                        RPerbaikan = Convert.ToString(row["RencanaPerbaikan"]),
                        Target = Convert.ToString(row["Target"]),
                        PIC = Convert.ToString(row["PIC"]),
                        TglActual = Convert.ToString(row["Tgl"])
                    });
            }
            return analisas;
        }
        public void AddAnalisa(AnalisaModels models,string Dept)
        {
            try
            {
                string query =
                @"INSERT INTO [IPMI_DB].[dbo].[tIpmiAnalisa]
                ([NoIPMI]
                ,[Penyebab]
                ,[RencanaPerbaikan]
                ,[Target]
                ,[PIC]
                ,[TglActual]
                ,[Status]
                ,InputByDept
                ,[CreatedBy]
                ,[CreatedDate])
                VALUES
                (   '" + models.NoIPMI + "' " +
                ",  '" + models.Penyebab + "' " +
                ",  '" + models.RPerbaikan + "' " +
                ",  '" + models.Target + "' " +
                ",  '" + models.PIC + "' " +
                ",  '" + models.Tgl + "' " +
                ",  'Closed' " +
                ",  '" + Dept + "' " +
                ",  '" + models.UserName + "' " +
                ",  GETDATE())";
                dbAccess.strConn = conn;
                dbAccess.ExecQuery(query);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateStatusIpmiDetail(string NoIpmi, string Dept)
        {
            try
            {
                string sql =
                @"UPDATE [tIPMIDetail]
                SET [Status] = 'Closed' 
                WHERE NoIPMI ='" + NoIpmi + "' AND Ke='" + Dept + "'";
                dbAccess.strConn = conn;
                dbAccess.ExecQuery(sql);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateStatusIpmi(string NoIpmi)
        {
            try
            {
                string sql =
                @"UPDATE [tIPMI]
                SET [Status] = 'Closed' 
                WHERE NoIPMI ='" + NoIpmi + "'";
                dbAccess.strConn = conn;
                dbAccess.ExecQuery(sql);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool IsAllClosed(string NoIpmi)
        {
            try
            {
                bool IsClosed = false;
                string sql =
                @"SELECT [Status]
                FROM [tIPMIDetail] WHERE NoIPMI ='" + NoIpmi + "'";

                DataTable dt = new DataTable();
                dbAccess.strConn = conn;
                dt= dbAccess.GetDataTable(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string status;
                    status = dt.Rows[i][0].ToString();

                    if (status=="Open")
                    {
                        IsClosed = false;
                        break;
                    }
                    else
                    {
                        IsClosed = true;
                    }
                }
                return IsClosed;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void EditIMDetails(AnalisaModels models, string userName)
        {
            try
            {
                string query =
                @"UPDATE [tIpmiAnalisa]
                SET [Penyebab] = '" + models.Penyebab + "'" +
                ",[RencanaPerbaikan] = '" + models.RPerbaikan + "'" +
                ",[Target] = '" + models.Target + "'" +
                ",[PIC] = '" + models.PIC + "'" +
                ",[TglActual] = '" + DateTime.Parse(models.Tgl) + "'" +
                ",[Status] = '" + models.Status + "'" +
                ",[UpdatedBy] = '" + userName + "'" +
                ",[UpdateDate] = GETDATE() " +
                "WHERE [NoIPMI] ='" + models.NoIPMI + "'";
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