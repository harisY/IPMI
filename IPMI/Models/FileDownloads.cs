using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace IPMI.Models
{
    public class FileDownloads
    {
        public List<FileInfo> GetFile(string NoIpmi)
        {
            List<FileInfo> listFiles = new List<FileInfo>();
            string fileSavePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/uploads");
            DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);
            int i = 0;
            foreach (var item in dirInfo.GetFiles("*" + NoIpmi + "*.*"))
            {
                listFiles.Add(new FileInfo()
                {
                    FileId = i + 1,
                    FileName = item.Name,
                    FilePath = dirInfo.FullName + @"\" + item.Name
                });
                i = i + 1;
            }
            return listFiles;
        }

        public List<FileModels> GetFile1(string NoIpmi, int Type)
        {
            List<FileModels> listFiles = new List<FileModels>();
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT Name, Data, ContentType FROM tFile WHERE NoIpmi=@NoIpmi AND Type =@Type";
                    cmd.Parameters.AddWithValue("@NoIpmi", NoIpmi);
                    cmd.Parameters.AddWithValue("@Type", Type);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            listFiles.Add(new FileModels()
                            {
                                Data = (byte[])sdr["Data"],
                                ContentType = sdr["ContentType"].ToString(),
                                Name = sdr["Name"].ToString()
                            });


                        }

                    }
                    con.Close();
                }
                return listFiles;
            }

        }
    }
}