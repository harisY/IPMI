using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IPMI.dtReport;
using IPMI.Services;

namespace IPMI.Models.Repo
{
    public class ReportIMrepo
    {
        private SqlConnection con;
        public static TransactionHelper gh_Trans;
        private IMService ObjIM;
        private DeptService ObjDept;
        public ReportIMrepo()
        {
            this.ObjIM = new IMService();
            this.ObjDept = new DeptService();
        }
        private static string Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            return constr;

        }

        public static dtReports GetDataSet(string pQuery, string dt, int pTimeOut = 300)
        {
            SqlDataAdapter da = null;
            dtReports dsa = new dtReports();
            try
            {
                if (gh_Trans != null && gh_Trans.Command != null)
                {
                    gh_Trans.Command.CommandType = CommandType.Text;
                    gh_Trans.Command.CommandText = pQuery;
                    gh_Trans.Command.CommandTimeout = pTimeOut;
                    da = new SqlDataAdapter(gh_Trans.Command);
                    da.Fill(dsa);
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection())
                    {
                        conn.ConnectionString = Connection();
                        conn.Open();
                        da = new SqlDataAdapter(pQuery, conn);
                        da.Fill(dsa, dt);
                    }
                }
                da = null;
                return dsa;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet LaporanIPMIbyDept(string IdDept)
        {
            try
            {
               
                dtReports ds = new dtReports();
                string Query;
                Query =
                        @"SELECT a.[NoIPMI]
                        ,a.[Tgl]
                        ,b.NamaDept Dari
                        ,a.[Masalah]
                        ,a.[Jumlah]
                        ,a.[Status]
                        FROM [tIpmi] a INNER JOIN mDept b on a.Dari = b.IdDept
                        WHERE a.Dari = COALESCE(NULLIF('" + IdDept + "','ALL'),a.Dari)";
                ds = GetDataSet(Query, "dtIpmi");
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet LaporanIPMIDetails(string NoIpmI)
        {
            try
            {

                dtReports ds = new dtReports();
                string Query;
                Query =
                        @"SELECT a.[NoIPMI]
                        ,b.NamaDept Ke
                        ,a.[Status]
                        FROM [tIpmidetail] a INNER JOIN mDept b on a.Ke = b.IdDept";
                //WHERE a.NoIPMI = '" + NoIpmI + "'
                ds = GetDataSet(Query, "dtIpmiDetails");
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet LaporanIPMIanalisa()
        {
            try
            {

                dtReports ds = new dtReports();
                string Query;
                Query =
                        @"SELECT a.[NoIPMI]
                        ,a.Penyebab
                        ,a.RencanaPerbaikan
                        ,a.Target
                        ,a.PIC
                        ,a.TglActual
                        ,b.NamaDept InputByDept
                        ,a.[Status]
                        FROM [tIpmiAnalisa] a INNER JOIN mDept b on a.InputByDept = b.IdDept";
                //WHERE a.NoIPMI = '" + NoIpmI + "'
                ds = GetDataSet(Query, "dtIpmiAnalisa");
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataSet LaporanIPMIanalisa_Subreport()
        {
            try
            {

                dtReports ds = new dtReports();
                string Query;
                Query =
                        @"SELECT [NoIPMI]
                        ,Penyebab
                        ,RencanaPerbaikan
                        ,Target
                        ,PIC
                        ,TglActual
                        ,[Status]
                        FROM [tIpmiAnalisa]";
                //WHERE a.NoIPMI = '" + NoIpmI + "'
                ds = GetDataSet(Query, "dtIpmiAnalisa");
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataSet LaporanIPMI(string Dari, string Ke, string From, string To)
        {
            try
            {
               

                dtReports ds = new dtReports();
                string Query;
                Query =
                @"SELECT a.[Id]
                    ,a.[NoIPMI]
                    ,a.[Tgl]
                    ,b.NamaDept [Dari]
                    ,(select namadept from mDept where Iddept= a.ke) Ke
                    ,a.[Masalah]
                    ,a.[Jumlah]
                    ,a.[Status]
                    ,a.[Ket]
                    ,a.[FileName]
                    ,a.[Status1]
                    ,a.[CreatedBy]
                    ,a.[CreatedDate]
                    ,a.[UpdatedBy]
                    ,a.[UpdateDate]
                FROM [tIpmi] a INNER JOIN
                mDept b on a.Dari = b.IdDept
                WHERE a.Dari =coalesce(NULLIF('" + Dari.Replace(@";", string.Empty) + "','ALL'),a.Dari)" +
                "   AND a.Ke =coalesce(NULLIF('" + Ke.Replace(@";", string.Empty) + "','ALL'),a.Ke)" +
                "   AND a.tgl >= coalesce(NULLIF('" + From.Replace(@";", string.Empty) + "','ALL'),a.tgl) AND a.tgl <= coalesce(NULLIF('" + To.Replace(@";", string.Empty) + "','ALL'),a.tgl)";
                ds = GetDataSet(Query, "dtIpmiNew");
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataSet LaporanIPMI_1(string Dari, string Ke, string From, string To)
        {
            try
            {
                dtReports ds = new dtReports();
                string Query;
                Query =
                @"SELECT a.[Id]
                  ,a.[NoIPMI]
                  ,a.[Tgl]
                  ,c.namaDept Dari
                  ,(select namadept from mDept where Iddept= a.ke) Ke
                  ,a.[Masalah]
                  ,a.[Status]
                  ,a.[Ket]
                  ,b.[Penyebab]
                  ,b.[RencanaPerbaikan]
                  ,b.[Target]
                  ,b.[PIC]
                  ,b.[TglActual]
                  ,b.[Status]
                FROM [dbo].[tIpmi] a Left JOIN
                [tIpmiAnalisa] b on a.NoIpmi = b.NoIpmi INNER JOIN
                mDept c on a.Dari = c.IdDept
                WHERE a.Dari =coalesce(NULLIF('" + Dari + "','ALL'),a.Dari)" +
                "   AND a.Ke =coalesce(NULLIF('" + Ke + "','ALL'),a.Ke)" +
                "   AND a.tgl >= coalesce(NULLIF('" + From.Replace(@";", string.Empty) + "','ALL'),a.tgl) AND a.tgl <= coalesce(NULLIF('" + To.Replace(@";", string.Empty) + "','ALL'),a.tgl)";
                ds = GetDataSet(Query, "dtTable1");
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}