using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IPMI.Models.IM;
using IPMI.Models.Repo;
using IPMI.Master;
using System.IO;

namespace IPMI.Controllers
{

    public class IMController : Controller
    {
        [Authorize]
        // GET: Dept
        public ActionResult Index()
        {
            try
            {
                IMRepos repos = new IMRepos();
                ModelState.Clear();
                var result = repos.getListIM("");
                return View(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /Dept/Edit/Create 
        //[Authorize(Roles = "Administrator, MIS")]
        public ActionResult Create()
        {
            IMModels iM = new IMModels();
            ModelState.Clear();
            ViewBag.IdDept = GetDeptToList();


            deptRepos deptrepos = new deptRepos();
            string Dept = deptrepos.getDeptByUser(User.Identity.Name);
            ViewBag.Dari = Dept;
            IMRepos repos = new IMRepos();
            string AutoNo = repos.AutoNo(Dept);
            ViewData["Dept"] = repos.GetDept();
            ViewBag.AutoNO = AutoNo;
            TempData["data"] = AutoNo;
            ViewData["ListDept"] = repos.GetDeptByUserLogin(User.Identity.Name);
            GetListIM();
            //GetListIMtDetail("0");
            //return View(iM);
            return View("Process");
        }

        public ActionResult GetListIM()
        {
            IMRepos repos = new IMRepos();
            ViewData["ListIM"] = repos.getListIM(User.Identity.Name);
            return PartialView("Partial/_GridProcess");
        }
        // POST: Dept
        //[Authorize(Roles = "Administrator, MIS")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(IMModels models)
        {
            try
            {
                IMRepos repo = new IMRepos();
                var CreatedBy = User.Identity.Name;
                var CreatedDate = DateTime.Now;
                var NoIpmi = models.NoIPMI.ToString();
                var Tgl = models.Tgl.ToString();
                var Dari = models.Dari.ToString();
                var Ke = models.Ke.ToString();
                var Masalah = models.Masalah.ToString();
                var Jumlah = models.Jumlah.ToString();
                var FileName = models.FileName.ToString();
                var ObjIM = new IMModels
                {
                    NoIPMI = NoIpmi,
                    Tgl = Tgl,
                    Dari = Dari,
                    Masalah = Masalah,
                    Jumlah = int.Parse(Jumlah),
                    FileName = FileName,
                    Ke = Ke,
                    CreatedBy = CreatedBy,
                    CreatedDate = CreatedDate
                    
                };

                
                repo.AddIM(ObjIM);

                deptRepos deptrepos = new deptRepos();
                string Dept = deptrepos.getDeptByUser(User.Identity.Name);
                ViewBag.Dari = Dept;
            }
            catch (Exception ex)
            {
                deptRepos deptrepos = new deptRepos();
                string Dept = deptrepos.getDeptByUser(User.Identity.Name);
                ViewBag.Dari = Dept;
                ModelState.AddModelError(string.Empty, ex.Message);
                //return View(iM);
            }
            return GetListIM();
        }
        // GET: /Dept/Edit/TestUser 
        //[Authorize(Roles = "Administrator, MIS")]
        public ActionResult Edit(string IdDept)
        {
            deptRepos repos = new deptRepos();

            try
            {
                if (IdDept == "")
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                var result = repos.getDeptByID(IdDept);
                if (result == null)
                    return HttpNotFound();
                return View(result.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator, MIS")]
        public ActionResult Update(IMModels models)
        {
            try
            {
                IMRepos repo = new IMRepos();
                var UpdateBy = User.Identity.Name;
                var UpdateDate = DateTime.Now;
                var NoIpmi = models.NoIPMI.ToString();
                var Tgl = models.Tgl.ToString();
                var Dari = models.Dari.ToString();
                var Ke = models.Ke.ToString();
                var Masalah = models.Masalah.ToString();
                var Jumlah = models.Jumlah.ToString();
                var FileName = models.FileName.ToString();
                var ObjIM = new IMModels
                {
                    NoIPMI = NoIpmi,
                    Tgl = Tgl,
                    Dari = Dari,
                    Masalah = Masalah,
                    Jumlah = int.Parse(Jumlah),
                    FileName = FileName,
                    Ke = Ke,
                    UpdateDate = UpdateDate,
                    UpdatedBy = UpdateBy

                };
                repo.EditIM(ObjIM);
                
                deptRepos deptrepos = new deptRepos();
                string Dept = deptrepos.getDeptByUser(User.Identity.Name);
                ViewBag.Dari = Dept;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                throw;
            }
            return GetListIM();

        }

        [HttpPost]
        //[Authorize(Roles = "Administrator, MIS")]
        public ActionResult Delete(string IdDept)
        {
            if (IdDept == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                deptRepos repos = new deptRepos();
                repos.DeleteDeptById(IdDept);
                return Json(new { success = true, message = "Delete Successfully" }, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }
        }

        private List<SelectListItem> GetDeptToList()
        {
            List<SelectListItem> SelectGroupListItems =
                new List<SelectListItem>();

            MasterRepository repos = new MasterRepository();
            var DeptSelectList = repos.GetDept();

            SelectGroupListItems.Add(
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                });

            foreach (var item in DeptSelectList)
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
        public ActionResult GetListIMtDetail(string Id)
        {
            IMRepos repos = new IMRepos();
            ViewData["ListIMDetail"] = repos.GetListRequestDetail(Id);
            return PartialView("Partial/_GridProcessDetail");
        }
        public ActionResult GetListIMtView(string Id)
        {
            IMRepos repos = new IMRepos();
            ViewData["ListIMDetail"] = repos.GetListRequestDetail(Id);
            return PartialView("Partial/_GridIMView");
        }
        public ActionResult Upload()
        {
            try
            {
                foreach (string upload in Request.Files)
                {
                    if (Request.Files[upload].FileName != "")
                    {
                        string path = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/uploads/";
                        string filename = Path.GetFileName(Request.Files[upload].FileName);
                        Request.Files[upload].SaveAs(Path.Combine(path, filename));
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}