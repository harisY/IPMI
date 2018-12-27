using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IPMI.Models;
using IPMI.Models.Analisa;
using IPMI.Models.Repo;


namespace IPMI.Controllers
{
    public class AnalisaController : Controller
    {
        [Authorize]
        // GET: Analisa
        public ActionResult Index()
        {
            deptRepos deptrepos = new deptRepos();
            string Dept = deptrepos.getDeptByUser(User.Identity.Name);

            ViewBag.Dept = Dept;
            AnalisaRepos repos = new AnalisaRepos();
            ViewData["ListAnalisa"] = repos.getListAnalisa(Dept);
            return View();
        }

        // GET: Analisa/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                deptRepos deptrepos = new deptRepos();
                string Dept = deptrepos.getDeptByUser(User.Identity.Name);

                AnalisaRepos repos = new AnalisaRepos();
                ViewData["ListAnalisaByNoIpmi"] = repos.getListAnalisaByNoIpmi(id, Dept);
                string query = (ViewData["ListAnalisaByNoIpmi"] ?? string.Empty) as string;
                if (query == null)
                {
                    //this.AddToastMessage("IPMI", "Data is empty, please click Add(+) button", ToastType.Warning);
                    RedirectToAction("Index","Analisa");
                    //return View("Index");
                }
                

                return View();
            }
            catch (Exception)
            {
                throw;
               
            }
            
        }

        // GET: Analisa/Create
        public ActionResult Create(string id)
        {
            ViewBag.NoIpmi = id;
            
            return View();
        }

        // POST: Analisa/Create
        [HttpPost]
        public ActionResult Create(AnalisaModels models)
        {
            try
            {
                var ObjAnalisa = new AnalisaModels {
                    NoIPMI = models.NoIPMI,
                    Penyebab = models.Penyebab,
                    RPerbaikan = models.RPerbaikan,
                    Target = models.Target,
                    PIC = models.PIC,
                    Tgl = models.Tgl,
                    UserName = User.Identity.Name

                };

                deptRepos deptrepos = new deptRepos();
                string Dept = deptrepos.getDeptByUser(User.Identity.Name);

                AnalisaRepos repos = new AnalisaRepos();
                repos.AddAnalisa(ObjAnalisa,Dept);
                            
            
                repos.UpdateStatusIpmiDetail(ObjAnalisa.NoIPMI.ToString(), Dept);

                bool IsStatusClosed = repos.IsAllClosed(ObjAnalisa.NoIPMI);
                if (IsStatusClosed)
                {
                    repos.UpdateStatusIpmi(ObjAnalisa.NoIPMI);
                }
                this.AddToastMessage("IPMI", "Save Successfully", ToastType.Success);
                return Json(new { success = true, message = "Update Successfully" }, JsonRequestBehavior.AllowGet);
                // TODO: Add insert logic here

                //return Redirect("~/Analisa/Index");
            }
            catch
            {
                this.AddToastMessage("IPMI", "Save failed", ToastType.Success);
                return View();
            }
        }

        // GET: Analisa/Edit/5
        public ActionResult Edit(string id)
        {
            deptRepos deptrepos = new deptRepos();
            string Dept = deptrepos.getDeptByUser(User.Identity.Name);
            AnalisaRepos repos = new AnalisaRepos();
            ViewData["ListAnalisaByNoIpmi"] = repos.getListAnalisaByNoIpmi(id,Dept);
            return View();
        }

        // POST: Analisa/Edit/5
        [HttpPost]
        public ActionResult Edit(AnalisaModels models)
        {
            try
            {
                AnalisaRepos repos = new AnalisaRepos();
                repos.EditIMDetails(models,User.Identity.Name);
                this.AddToastMessage("IPMI", "Update Successfully", ToastType.Success);
                return Json(new { success = true, message = "Update Successfully" }, JsonRequestBehavior.AllowGet);

                //return RedirectToAction("Index");
            }
            catch
            {
                this.AddToastMessage("IPMI", "Update Failed", ToastType.Success);
                return View();
            }
        }

        // GET: Analisa/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Analisa/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public FileResult Download(string FileName)
        {

            //string FileName = "Catatan training.xlsx";
            var FileVirtualPath = "~/App_Data/uploads/" + FileName;
            
            //return File(FileVirtualPath, contentType, FileVirtualPath);
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
            //return File(FileVirtualPath, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
        }
    }
}
