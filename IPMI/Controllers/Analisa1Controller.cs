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
using System.IO.Compression;
using System.Configuration;
using System.Data.SqlClient;
using Ionic.Zip;

namespace IPMI.Controllers
{
    public class Analisa1Controller : Controller
    {
        private IMService objIpmi;
        private DeptService objDept;
        private AnalisaService ObjAnalisa;
        public Analisa1Controller()
        {
            this.objIpmi = new IMService();
            this.objDept = new DeptService();
            this.ObjAnalisa = new AnalisaService();
        }
        // GET: Analisa1
        public ActionResult Index()
        {
            object[] nama = { User.Identity.Name };
            string NamaDept = objDept.GetDeptByID(nama);
            ViewBag.Dari = NamaDept;
            object[] dept = { NamaDept };
            var result = ObjAnalisa.GetAll(dept);
            return View(result);
        }

        // GET: Analisa1/Details/5
        public ActionResult Details(string id, string state)
        {
            ViewBag.state = state;
            object[] parameters = { id };
            ViewData["ListAnalisa"] = ObjAnalisa.GetbyID(parameters);
            return View();
        }

        // GET: Analisa1/Create
        public ActionResult Create(string id)
        {
            ViewBag.NoIpmi = id;
            return View();
        }

        // POST: Analisa1/Create
        [HttpPost]
        public ActionResult Create(tIpmiAnalisa models)
        {
            int result = 0;
            int result1 = 0;
            models.CreatedBy = User.Identity.Name;
            object[] parameters = { models.NoIPMI, models.Penyebab
                                        ,models.RencanaPerbaikan, models.Target
                                        ,models.PIC, models.TglActual
                                        ,models.CreatedBy};
            result = ObjAnalisa.Insert(parameters);
            
            if (result == 1)
            {
                object[] param = { models.NoIPMI };
                result1 = objIpmi.UpdateStatusIpmi(param);
                if (result1 == 1)
                {
                    this.AddToastMessage("IPMI", "Data berhasil di simpan !", ToastType.Success);
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    this.AddToastMessage("IPMI", "update status Ipmi failed !", ToastType.Error);
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
                
            }
            else
            {
                this.AddToastMessage("IPMI", "Data Tidak Berhasil di simpan !", ToastType.Error);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Analisa1/Edit/5
        public ActionResult Edit(string id)
        {
            object[] parameters = { id };
            ViewData["ListAnalisa"] = ObjAnalisa.GetbyID(parameters);
            return View();
        }

        // POST: Analisa1/Edit/5
        [HttpPost]
        public ActionResult Edit(tIpmiAnalisa models)
        {
            int result = 0;
            int result1 = 0;
            models.UpdatedBy = User.Identity.Name;
            object[] parameters = { models.NoIPMI, models.Penyebab
                                        ,models.RencanaPerbaikan, models.Target
                                        ,models.PIC, models.TglActual
                                        ,models.CreatedBy};
            result = ObjAnalisa.Update(parameters);
            if (result == 1)
            {
                object[] param = { models.NoIPMI,models.TglActual };
                result1 = ObjAnalisa.UpdateStatusIpmi(param);
                if (result1 == 1)
                {
                    this.AddToastMessage("IPMI", "Data berhasil di update !", ToastType.Success);
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    this.AddToastMessage("IPMI", "update status Ipmi failed !", ToastType.Error);
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
                
            }
            else
            {
                this.AddToastMessage("IPMI", "Data tidak berhasil di update !", ToastType.Error);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Analisa1/Delete/5
        public ActionResult Delete(string id)
        {
            object[] parameters = { id };
            ViewData["ListAnalisa"] = ObjAnalisa.GetbyID(parameters);
            return View();
        }

        // POST: Analisa1/Delete/5
        [HttpPost]
        public ActionResult Delete(tIpmiAnalisa models)
        {
            int result = 0;
            int result1 = 0;
            object[] parameters = { models.NoIPMI };
            result = ObjAnalisa.Delete(parameters);
            if (result == 1)
            {
                object[] param = { models.NoIPMI };
                result1 = objIpmi.UpdateStatusIpmi_Open(param);
                if (result1 == 1)
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
            else
            {
                this.AddToastMessage("IPMI", "Data tidak berhasil di hapus !", ToastType.Error);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
      
        public ActionResult Download(string FileName)
        {
            FileDownloads obj = new FileDownloads();
            //////int CurrentFileID = Convert.ToInt32(FileID);  
            var filesCol = obj.GetFile(FileName).ToList();
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < filesCol.Count; i++)
                    {
                        ziparchive.CreateEntryFromFile(filesCol[i].FilePath, filesCol[i].FileName);
                    }
                }
                return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
            }
        }
        #region ONPROGRESS
        public ActionResult Index1()
        {
            object[] nama = { User.Identity.Name };
            string NamaDept = objDept.GetDeptByID(nama);
            object[] dept = { NamaDept };
            var result = ObjAnalisa.GetAll_OnProgress(dept);
            return View(result);
        }
        #endregion

        #region CLOSED
        public ActionResult Index2()
        {
            object[] nama = { User.Identity.Name };
            string NamaDept = objDept.GetDeptByID(nama);
            object[] dept = { NamaDept };
            var result = ObjAnalisa.GetAll_Closed(dept);
            return View(result);
        }
        #endregion

        #region GLOBAL
        public ActionResult Index3()
        {
           
            var result = ObjAnalisa.GetAll1();
            return View(result);
        }
        #endregion

        private static List<FileModels> GetFiles(string NoIpmi)
        {
            List<FileModels> files = new List<FileModels>();
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT id, Name FROM tFile where NoIpmi= '" + NoIpmi + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            files.Add(new FileModels
                            {
                                Id = Convert.ToInt32(sdr["id"]),
                                Name = sdr["Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return files;
        }
        byte[] bytes;
        string fileName, contentType;
        [HttpGet]
        public FileResult DownloadFile(string NoIpmi)
        {
            
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT Name, Data, ContentType FROM tFile WHERE NoIpmi=@NoIpmi";
                    cmd.Parameters.AddWithValue("@NoIpmi", NoIpmi);
                    //cmd.Parameters.AddWithValue("@Type", Type);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            bytes = (byte[])sdr["Data"];
                            contentType = sdr["ContentType"].ToString();
                            fileName = sdr["Name"].ToString();
                            
                        }
                       
                    }
                    con.Close();
                }

            }
            return File(bytes, contentType, fileName);

        }
        public ActionResult DownloadFromDB(string FileName)
        {
            FileDownloads obj = new FileDownloads();
            //////int CurrentFileID = Convert.ToInt32(FileID);  
            var filesCol = obj.GetFile1(FileName,0).ToList();
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < filesCol.Count; i++)
                    {
                        ziparchive.CreateEntryFromFile(filesCol[i].Name, filesCol[i].Name);
                    }
                }
                return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
            }
        }
        public FileResult DownloadMultipleFiles(string NoIpmi,int Type)
        {
            FileDownloads obj = new FileDownloads();
            var filesCol = obj.GetFile1(NoIpmi,Type).ToList();
            using (MemoryStream ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var file in filesCol)
                    {
                        var entry = archive.CreateEntry(file.Name, CompressionLevel.Fastest);
                        using (var zipStream = entry.Open())
                        {
                            zipStream.Write(file.Data, 0, file.Data.Length);
                        }
                    }
                }

                return File(ms.ToArray(), "application/zip", "Attachments.zip");
            }
        }
        //public ActionResult DownloadMultipleFiles1(string NoIpmi, int Type)
        //{
        //    try
        //    {
                
        //        FileDownloads obj = new FileDownloads();
        //        var filesCol = obj.GetFile1(NoIpmi, Type).ToList();

        //        Response.Clear();

        //        var downloadFileName = string.Format("YourDownload-{0}.zip", DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss"));
        //        Response.ContentType = "application/zip";

        //        Response.AddHeader("content-disposition", "filename=" + downloadFileName);

        //        using (Ionic.Zip.ZipFile zipFile = new Ionic.Zip.ZipFile())
        //        {
        //            zipFile.AddDirectoryByName("Files");
        //            foreach (var file in filesCol)
        //            {
        //                zipFile.AddFile(file.Name, "Files");
        //            }
        //            zipFile.Save(Response.OutputStream);

        //            return File(zipFile.GetStream(), "application/octet-stream", downloadFileName);
        //        }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
