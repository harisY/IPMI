using IPMI.Models.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IPMI.Models;
using IPMI.Entity;
using IPMI.Services;
using IPMI.Repository;
namespace IPMI.Controllers
{
    public class ReportsController : Controller
    {
        private DeptService objDept;

        public ReportsController()
        {
            this.objDept = new DeptService();
        }
        // GET: Reports
        public ActionResult Index()
        {
            deptRepos dept = new deptRepos();
            ModelState.Clear();
            var result = dept.getListDepts();
            return View(result);
        }
        public ActionResult Index1()
        {

            List<ReportModels> models = new List<ReportModels>() {
                new ReportModels(){FieldName = "Dari (Departemen)", Type = 1},
                new ReportModels(){FieldName = "Ke (Departemen)", Type = 2},
                new ReportModels(){FieldName = "Tanggal", Type = 3}
            };
            
            
            return View(models);
        }
        public ActionResult IndexAnalisa()
        {
            deptRepos dept = new deptRepos();
            ModelState.Clear();
            var result = dept.getListDepts();
            return View(result);
        }
        [HttpPost]
        public ActionResult ShowLaporanIpmiByDept(string id)
        {

            ReportIMrepo report = new ReportIMrepo();
            this.HttpContext.Session["ReportNameIM"] = "ReportIM.rpt";
            this.HttpContext.Session["ReportIM"] = report.LaporanIPMIbyDept(id);
            this.HttpContext.Session["ReportIMDetails"] = report.LaporanIPMIDetails("");

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ShowLaporanAnalisaIpmiByDept(string id)
        {

            ReportIMrepo report = new ReportIMrepo();
            this.HttpContext.Session["ReportNameIManalisa"] = "ReportIManalisa.rpt";
            this.HttpContext.Session["ReportIManalisa"] = report.LaporanIPMIbyDept(id);
            this.HttpContext.Session["ReportIManalisaSub"] = report.LaporanIPMIanalisa();

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetParameter1(string FieldName)
        {

            var result = GetDeptToList();
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ShowLaporanAIPMI_New(string Dari, string Ke, string From, string To )
        {


            if (From.Replace(@";", string.Empty) == "" || From == null)
            {
                From = "ALL";
            }
            if (To.Replace(@";", string.Empty) == "" || To == null)
            {
                To = "ALL";
            }
            ReportIMrepo report = new ReportIMrepo();
            this.HttpContext.Session["ReportNameIM"] = "ReportIM_1.rpt";
            this.HttpContext.Session["ReportIM"] = report.LaporanIPMI_1(Dari, Ke, From, To);
            //this.HttpContext.Session["ReportIMDetails"] = report.LaporanIPMIanalisa_Subreport();

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }
        private List<SelectListItem> GetDeptToList()
        {
            List<SelectListItem> SelectGroupListItems =
                new List<SelectListItem>();

            List<StandartComboBox> standartCombos = new List<StandartComboBox>();
            var dept = objDept.GetAll();
            foreach (var item in dept)
            {
                standartCombos.Add(
                    new StandartComboBox
                    {
                        Value = item.IdDept,
                        Text = item.NamaDept
                    });
            }

            SelectGroupListItems.Add(
                new SelectListItem
                {
                    Text = "ALL DEPARTEMEN",
                    Value = "ALL"
                });

            foreach (var item in standartCombos)
            {
                SelectGroupListItems.Add(
                    new SelectListItem
                    {
                        Text = item.Text.ToString(),
                        Value = item.Value.ToString()
                    });
            }

            return SelectGroupListItems;
        }
    }

}
