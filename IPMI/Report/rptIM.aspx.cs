using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IPMI.Report
{
    public partial class rptIM : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                bool isValid = true;
                string strReportName = System.Web.HttpContext.Current.Session["ReportNameIM"].ToString();
                var rptSource = System.Web.HttpContext.Current.Session["ReportIM"];
                //var rptDetailSource = System.Web.HttpContext.Current.Session["ReportIMDetails"];
                if (string.IsNullOrEmpty(strReportName))
                {
                    isValid = false;
                }
                if (isValid)
                {
                    ReportDocument rd = new ReportDocument();
                    //string strRptPath = Server.MapPath("~/") + "/Report/IM//" + strReportName;
                    rd.Load(Server.MapPath("../Report/IM/" + strReportName));
                    //rd.Load(strRptPath);
                    //rd.SetDatabaseLogon("sa", "fid123!!");
                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                    {
                        rd.SetDataSource(rptSource);
                        //rd.Subreports[0].SetDataSource(rptDetailSource);
                    }
                        
                    
                    CrystalReportViewer1.ReportSource = rd;
                }
                else
                {
                    Response.Write("<H2>Nothing Found; No Report name found</H2>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
    }
}