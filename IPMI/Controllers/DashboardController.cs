using IPMI.Models;
using IPMI.Models.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IPMI.Services;

namespace IPMI.Controllers
{
    public class DashboardController : Controller
    {
        private DashboardService ObjDashboard;
        private DeptService objDept;
        public DashboardController()
        {
            this.ObjDashboard = new DashboardService();
            this.objDept = new DeptService();
        }
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboardv1()
        {
            List<string> deptid = new List<string>();
            List<int> data1 = new List<int>();
            List<int> data2 = new List<int>();

            object[] nama = { User.Identity.Name };
            string NamaDept = objDept.GetDeptByID(nama);
            ViewData["ListPie"] = ObjDashboard.GetPie(nama);
            //ViewData["ListBarChart"] = ObjDashboard.GetBarChart();

            var Obj = ObjDashboard.GetBarChart();

            foreach (var item in Obj)
            {
                deptid.Add(item.IdDept);
                data1.Add(item.Open);
                data2.Add(item.Closed);
            }

            ViewBag.label = deptid;
            ViewBag.data1 = data1;
            ViewBag.data2 = data2;
            object[] dept = { NamaDept };
            var result = ObjDashboard.GetAll(dept);
            return View(result);
        }

        public ActionResult Dashboardv2()
        {
            return View();
        }

        public ActionResult GetMessages()
        {
            MessagesRepository _messageRepository = new MessagesRepository();
            //return PartialView("_MessagesList", _messageRepository.GetAllMessages());
            deptRepos deptrepos = new deptRepos();
            string Dept = deptrepos.getDeptByUser(User.Identity.Name);
            var data = _messageRepository.GetAllMessages(Dept);
            string Jumlah = "0";
            foreach (var item in data)
            {
                Jumlah = item.Jumlah;
            }
            return Json(new {jumlah = Jumlah, success = true, message = "Update Successfully" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCompletTask()
        {
            MessagesRepository _messageRepository = new MessagesRepository();
            //return PartialView("_MessagesList", _messageRepository.GetAllMessages());
            deptRepos deptrepos = new deptRepos();
            string Dept = deptrepos.getDeptByUser(User.Identity.Name);
            var data = _messageRepository.GetCompletedTask(Dept);
            string Jumlah = "0";
            foreach (var item in data)
            {
                Jumlah = item.Jumlah;
            }
            return Json(new { jumlah = Jumlah, success = true, message = "Update Successfully" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCreatedTask()
        {
            MessagesRepository _messageRepository = new MessagesRepository();
            //return PartialView("_MessagesList", _messageRepository.GetAllMessages());
            deptRepos deptrepos = new deptRepos();
            string Dept = deptrepos.getDeptByUser(User.Identity.Name);
            var data = _messageRepository.GetCreatedTask(Dept);
            string Jumlah = "0";
            foreach (var item in data)
            {
                Jumlah = item.Jumlah;
            }
            return Json(new { jumlah = Jumlah, success = true, message = "Update Successfully" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCreatedTaskCompleted()
        {
            MessagesRepository _messageRepository = new MessagesRepository();
            //return PartialView("_MessagesList", _messageRepository.GetAllMessages());
            deptRepos deptrepos = new deptRepos();
            string Dept = deptrepos.getDeptByUser(User.Identity.Name);
            var data = _messageRepository.GetCreatedTaskCompleted(Dept);
            string Jumlah = "0";
            foreach (var item in data)
            {
                Jumlah = item.Jumlah;
            }
            return Json(new { jumlah = Jumlah, success = true, message = "Update Successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}