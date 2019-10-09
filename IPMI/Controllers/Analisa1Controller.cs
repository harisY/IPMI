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
using ICSharpCode.SharpZipLib.Zip;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Data;

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
            //string query = (ViewData["ListAnalisa"] ?? string.Empty) as string;
            //if (string.IsNullOrEmpty(query))
            //{
            //    this.AddToastMessage("IPMI", "Silahkan Edit dulu data di tab buat baru !", ToastType.Warning);
            //    return null;
            //}
            //else
            //{
            //    return View();
            //}            
        }

        // POST: Analisa1/Edit/5
        [HttpPost]
        public ActionResult Edit(tIpmiAnalisa models, bool IsExist)
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
                    if (IsExist)
                    {
                        ObjAnalisa.UpdateFileAnalisa(models.NoIPMI);
                    }

                    string Dari = string.Empty;
                    string Masalah = string.Empty;
                    string Ke = string.Empty;
                    DataTable dt = new DataTable();
                    dt = objIpmi.GetIpmiByNo(models.NoIPMI);
                    if (dt.Rows.Count > 0)
                    {
                        Dari = dt.Rows[0][0].ToString();
                        Masalah = dt.Rows[0][2].ToString();
                        Ke = dt.Rows[0][1].ToString();

                        string email = string.Empty;
                        email = objIpmi.GetEmailAnalisa(Dari);

                        //MailMessage mail = new MailMessage("ipmi@tsmu.co.id", email);
                        MailMessage mail = new MailMessage();
                        mail.IsBodyHtml = true;
                        mail.From = new MailAddress("ipmi@tsmu.co.id", "IPMI");
                        mail.To.Add(new MailAddress(email));
                        var smpt = new SmtpClient
                        {
                            Host = "mail.tsmu.co.id",
                            Port = 25,
                            EnableSsl = false,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            Credentials = new NetworkCredential("ipmi@tsmu.co.id", "svPyyO(++6cj"),
                            Timeout = 20000
                        };
                        var body = new StringBuilder();
                        body.AppendFormat("Analisa dan Perbaikan masalah :");
                        body.AppendLine(@"<br />>Dari =" + Ke);
                        body.AppendLine(@"<br />>Masalah =" + Masalah);
                        body.AppendLine(@"<br />>Penyebab =" + models.Penyebab);
                        body.AppendLine(@"<br />>Rencana Perbaikan =" + models.RencanaPerbaikan);
                        body.AppendLine("<br /><a href='https://srv02.tsmu.co.id/ipmi/Account/Login?ReturnUrl=%2Fipmi%2F'>Klik di sini untuk Login</a>");
                        mail.Body = body.ToString();
                        string emailSubject = "IPMI Notification " + models.NoIPMI;
                        mail.Subject = emailSubject;
                        //mail.Body = @"Masalah sudah di analisa : <br /> > Dari =" + models.Dari + "<br /> > Msalah = " + models.Masalah + "<br /> <a href='https://srv02.tsmu.co.id/ipmi/Account/Login?ReturnUrl=%2Fipmi%2F'>Klik di sini untuk Login</a>";
                        mail.IsBodyHtml = true;
                        mail.CC.Add("log@tsmu.co.id");
                        smpt.Send(mail);
                    }
                  
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
            ObjAnalisa = new AnalisaService();
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

        public ActionResult IndexDownload(string NoIpmi, int Type)
        {
            ViewBag.NoIpmi = NoIpmi;
            ViewBag.Type = Type;
            return View();
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
            try
            {
                FileDownloads obj = new FileDownloads();
                var filesCol = obj.GetFile1(NoIpmi, Type).ToList();
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

                    return File(ms.ToArray(), "application/octet-stream", "Attachments.zip");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        [HttpPost]
        public ActionResult DownloadMultipleFiles1(string NoIpmi, int Type)
        {
            try
            {
                FileDownloads obj = new FileDownloads();
                var filesCol = obj.GetFile1(NoIpmi, Type).ToList();
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

                    return File(ms.ToArray(), "application/octet-stream", NoIpmi +".zip");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        public ActionResult ZipDownload(string NoIpmi, int Type)
        {
            FileDownloads obj = new FileDownloads();
            var filesCol = obj.GetFile1(NoIpmi, Type).ToList();
            Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile();
            foreach (var file in filesCol)
            {

                zip.AddEntry(file.Name, (byte[])file.Data.ToArray());
            }
            var zipMs = new MemoryStream();
            zip.Save(zipMs);
            byte[] fileData = zipMs.GetBuffer();
            zipMs.Seek(0, SeekOrigin.Begin);
            zipMs.Flush();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Attachments.zip ");
            Response.ContentType = "application/zip";
            Response.BinaryWrite(fileData);
            Response.End();
            return new EmptyResult();
        }
        public FileResult DownloadFiles(string NoIpmi, int Type)
        {
            //Define file Type
            string fileType = "application/octet-stream";
            //string fileType = "application/zip";
            //Define Output Memory Stream
            var outputStream = new MemoryStream();

            FileDownloads obj = new FileDownloads();
            var filesCol = obj.GetFile1(NoIpmi, Type).ToList();
            //Create object of ZipFile library
            using (Ionic.Zip.ZipFile zipFile = new Ionic.Zip.ZipFile())
            {
               
                foreach (var file in filesCol)
                {
                  
                    zipFile.AddEntry(file.Name, (byte[])file.Data.ToArray());
                }

                Response.ClearContent();
                Response.ClearHeaders();

                //Set zip file name
                //Response.AppendHeader("content-disposition", "attachment; filename=Attachments.zip");
                Response.AddHeader("content-disposition", "attachment; filename=Attachments.zip");
                //Save the zip content in output stream
                Int64 fileSizeInBytes = filesCol.Count;
                Response.AddHeader("Content-Length", fileSizeInBytes.ToString());
                zipFile.Save(outputStream);
            }

            //Set the cursor to start position
            outputStream.Position = 0;

            //Dispance the stream
            return new FileStreamResult(outputStream, fileType);
        }

        [HttpPost]
        public ActionResult UploadToDB(List<HttpPostedFileBase> postedFile, string NoIpmi, bool New)
        {

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                if (New)
                {
                    string que = "Delete from tfile where NoIpmi=@No And Type = 2";
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
                                cmd.Parameters.AddWithValue("@Type", 2);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Type", 3);
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
