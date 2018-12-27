using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IPMI.Entity;
using IPMI.Repository;
using IPMI.Services;
using IPMI.Models;
using IPMI.Models.Repo;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

namespace IPMI.Controllers
{
    public class IpmiController : Controller
    {
        private IMService objIpmi;
        private DeptService objDept;
        private AnalisaService ObjAnalisa;
        public IpmiController()
        {
            this.objIpmi = new IMService();
            this.objDept = new DeptService();
            this.ObjAnalisa = new AnalisaService();
        }
        // GET: Ipmi
        public ActionResult Index()
        {
            object[] nama = { User.Identity.Name };
            string NamaDept = objDept.GetDeptByID(nama);
            ViewBag.Dari = NamaDept;
            object[] dept = { NamaDept };
            var result = objIpmi.GetAll(dept);
            
            return View(result);
        }

        // GET: Ipmi/Details/5
        public ActionResult Details(string id)
        {
            ViewBag.IdDept = GetDeptToList();
            object[] parameters = { id };
            ViewData["ListIpmi"] = objIpmi.GetbyID(parameters);
            return View();
        }

        // GET: Ipmi/Create
        public ActionResult Create()
        {
            ViewBag.IdDept = GetDeptToList();
            object[] nama = { User.Identity.Name };
            string NamaDept = objDept.GetDeptByID(nama);
            ViewBag.Dari = NamaDept;
            object[] dept = { NamaDept };
            ViewBag.AutoNO = objIpmi.AutoNo(dept);

            return View();
        }

        // POST: Ipmi/Create
        [HttpPost]
        public ActionResult Create(tIpmi model)
        {
            int result = 0;

            model.CreatedBy = User.Identity.Name;
            object[] parameters = { model.NoIPMI, model.Tgl
                                        ,model.Dari, model.ke
                                        ,model.Masalah, model.Jumlah
                                        ,model.FileName
                                        ,model.CreatedBy, model.FileName1};
            result = objIpmi.Insert(parameters);
            if (result == 1)
            {
                this.AddToastMessage("IPMI", "Data berhasil di simpan !", ToastType.Success);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                this.AddToastMessage("IPMI", "Data Tidak Berhasil di simpan !", ToastType.Error);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        // GET: Ipmi/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.IdDept = GetDeptToList();
            object[] parameters = { id };
            ViewData["ListIpmi"] = objIpmi.GetbyID(parameters);
            return View();
        }

        // POST: Ipmi/Edit/5
        [HttpPost]
        public ActionResult Edit(tIpmi models)
        {
            int result = 0;

            try
            {
                models.UpdatedBy = User.Identity.Name;
                object[] parameters = { models.NoIPMI, models.Tgl
                                        ,models.Dari, models.ke
                                        ,models.Masalah, models.Jumlah
                                        ,models.FileName
                                        ,models.UpdatedBy, models.FileName1};
                result = objIpmi.Update(parameters);
                if (result == 1)
                {

                    this.AddToastMessage("IPMI", "Data berhasil di update !", ToastType.Success);
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    this.AddToastMessage("IPMI", "Data tidak berhasil di update !", ToastType.Error);
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }

            
        }

        // GET: Ipmi/Delete/5
        public ActionResult Delete(string id)
        {
            ViewBag.IdDept = GetDeptToList();
            object[] parameters = { id };
            ViewData["ListIpmi"] = objIpmi.GetbyID(parameters);
            return View();
        }

        // POST: Ipmi/Delete/5
        [HttpPost]
        public ActionResult Delete(tIpmi models)
        {
            int result = 0;

            object[] parameters = { models.NoIPMI};
            result = objIpmi.Delete(parameters);
            if (result == 1)
            {

                this.AddToastMessage("IPMI", "Data berhasil di hapus !", ToastType.Success);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                this.AddToastMessage("IPMI", "Data tidak berhasil di hapus !", ToastType.Error);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        #region VERIFIKASI
        public ActionResult Index3()
        {
            object[] nama = { User.Identity.Name };
            string NamaDept = objDept.GetDeptByID(nama);
            ViewBag.Dari = NamaDept;
            object[] dept = { NamaDept };
            var result = objIpmi.Get_IPMI_Verifikasi(dept);

            return View(result);
        }
        public ActionResult Verifikasi(string id)
        {
            ViewBag.IdDept = GetDeptToList();
            object[] parameters = { id };
            ViewData["ListIpmi"] = objIpmi.GetbyID(parameters);
            ViewData["ListAnalisa"] = ObjAnalisa.GetbyID(parameters);
            return View();
        }
        [HttpPost]
        public ActionResult Verifikasi(tIpmi models)
        {
            int result = 0;
            object[] parameters = { models.NoIPMI};
            result = objIpmi.Update_Status_Ipmi_Closed(parameters);
            if (result == 1)
            {

                this.AddToastMessage("IPMI", "Data berhasil di update !", ToastType.Success);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                this.AddToastMessage("IPMI", "Data tidak berhasil di update !", ToastType.Error);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UnVerifikasi(tIpmi models, string FileName1)
        {
            int result = 0;
            object[] parameters = { models.NoIPMI, models.Ket, FileName1 };
            result = objIpmi.SetHistoryIpmi_UpdateStatus1(parameters);
            if (result == 1)
            {

                this.AddToastMessage("IPMI", "Data berhasil di update !", ToastType.Success);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                this.AddToastMessage("IPMI", "Data tidak berhasil di update !", ToastType.Error);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region CloseTask
        public ActionResult Index1()
        {
            object[] nama = { User.Identity.Name };
            string NamaDept = objDept.GetDeptByID(nama);
            ViewBag.Dari = NamaDept;
            object[] dept = { NamaDept };
            var result = objIpmi.GetIpmiClosed(dept);

            return View(result);
        }
        public ActionResult Details1(string id)
        {
            ViewBag.IdDept = GetDeptToList();
            object[] parameters = { id };
            ViewData["ListIpmi"] = objIpmi.GetbyID(parameters);
            return View();
        }
        #endregion

        #region GLOBAL
        public ActionResult Index2()
        {
            object[] nama = { User.Identity.Name };
            string NamaDept = objDept.GetDeptByID(nama);
            ViewBag.Dari = NamaDept;
            var result = objIpmi.GetAll();

            return View(result);
        }
        public ActionResult Details2(string id)
        {
            ViewBag.IdDept = GetDeptToList();
            object[] parameters = { id };
            ViewData["ListIpmi"] = objIpmi.GetbyID(parameters);
            return View();
        }
        #endregion
        private List<SelectListItem> GetDeptToList()
        {
            List<SelectListItem> SelectGroupListItems =
                new List<SelectListItem>();

            List<StandartComboBox> standartCombos = new List<StandartComboBox>();
            object[] Username = { User.Identity.Name };
            var dept = objDept.GetAll(Username);
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
                    Text = "PILIH DEPARTEMEN",
                    Value = "0"
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
        public ActionResult Upload()
        {
            try
            {
                foreach (string upload in Request.Files)
                {
                    if (Request.Files[upload].FileName != "")
                    {
                        string path = AppDomain.CurrentDomain.BaseDirectory + "/Content/uploads/";
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

        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase[] files, string NoIpmi)
        {

            //Ensure model state is valid
            if (ModelState.IsValid)
            {   //iterating through multiple file collection 
                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/App_Data/uploads/") + InputFileName);
                        //Save file to server folder
                        file.SaveAs(ServerSavePath);
                        //assigning file uploaded status to ViewBag for showing message to user.
                        //ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
                    }


                }
            }
            ViewBag.IdDept = GetDeptToList();
            object[] nama = { User.Identity.Name };
            string NamaDept = objDept.GetDeptByID(nama);
            ViewBag.Dari = NamaDept;
            object[] dept = { NamaDept };
            ViewBag.AutoNO = objIpmi.AutoNo(dept);

            return RedirectToAction("Create");

        }

        [HttpPost]
        public ActionResult UploadToDB(List<HttpPostedFileBase> postedFile, string NoIpmi, bool New)
        {

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                if (New)
                {
                    string que = "Delete from tfile where NoIpmi=@No And Type = 0";
                    using (SqlCommand cmd = new SqlCommand(que))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@No", NoIpmi);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

            }

            foreach (HttpPostedFileBase File in postedFile)
            {
                if (File != null)
                {
                    byte[] bytes;
                    using (BinaryReader br = new BinaryReader(File.InputStream))
                    {
                        bytes = br.ReadBytes(File.ContentLength);
                    }
                    string fileName = Path.GetFileName(File.FileName);                    
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        string query = "INSERT INTO tFile(NoIpmi,Name,ContentType,Data,Type) VALUES (@NoIpmi,@Name, @ContentType, @Data,@Type)";
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@NoIpmi", NoIpmi);
                            cmd.Parameters.AddWithValue("@Name", fileName);
                            cmd.Parameters.AddWithValue("@ContentType", File.ContentType);
                            cmd.Parameters.AddWithValue("@Data", bytes);
                            if (New)
                            {
                                cmd.Parameters.AddWithValue("@Type", 0);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Type", 1);
                            }
                            
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }

            return null;
        }
    }
}
