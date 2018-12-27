using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IPMI.Models;
using IPMI.Models.Master;
using IPMI.Models.Repo;

namespace IPMI.Controllers
{
    public class DeptController : Controller
    {
        // GET: Dept
        public ActionResult Index()
        {
            try
            {
                deptRepos dept = new deptRepos();
                ModelState.Clear();
                var result = dept.getListDepts();
                return View(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /Dept/Edit/Create 
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            
            deptModels dept = new deptModels();
            ModelState.Clear();

            return View(dept);
        }

        // POST: Dept
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(deptModels dept)
        {
            try
            {
                deptRepos repo = new deptRepos();
                if (dept == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var IdDept = dept.IdDept.Trim().ToUpper();
                var NamaDept = dept.NamaDept.Trim().ToUpper();

                if (IdDept == null || IdDept == "") throw new Exception("ID Dept empty !");

                if (NamaDept == "") throw new Exception("Nama Department empty !");

                bool isValid = repo.isDeptExist(IdDept);
                if (isValid)
                {
                    throw new Exception("Data Exist !");
                }
                
                var ObjDept = new deptModels { IdDept = IdDept, NamaDept = NamaDept };
                
                repo.AddDept(ObjDept);
                this.AddToastMessage("", "Data saved", ToastType.Success);
                return Redirect("~/Dept/Index");

            }
            catch (Exception ex)
            {
                this.AddToastMessage("Error", ex.Message, ToastType.Error);
                //ModelState.AddModelError(string.Empty, ex.Message);
                return View(dept);
            }
        }
        // GET: /Dept/Edit/TestUser 
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string IdDept)
        {
            deptRepos repos = new deptRepos();

            try
            {
                if (IdDept == "")
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                var result = repos.getDeptByID(IdDept);
                if(result == null)
                    return HttpNotFound();
                return View(result.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Update(deptModels dept)
        {
            if (dept.IdDept == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                deptRepos repos = new deptRepos();
                repos.EditDept(dept);
                return Json(new { success = true, message = "Update Successfully" }, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(string IdDept)
        {
            try
            {
                if (IdDept == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    deptRepos repos = new deptRepos();
                    bool IsExist = repos.IsDataExist(IdDept);
                    if (IsExist)
                    {
                        throw new Exception("Data already declare in User Table, cannot be deleted !");
                       
                    }
                    repos.DeleteDeptById(IdDept);
                    this.AddToastMessage("IPMI", "Delete Successfully", ToastType.Success);
                    return Json(new { success = true, message = "Delete Successfully" }, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //this.AddToastMessage("IPMI", ex.Message, ToastType.Warning);
                return Json(new { success = false, message = ex.Message}, JsonRequestBehavior.AllowGet);
                throw;
            }
            
        }
    }
}