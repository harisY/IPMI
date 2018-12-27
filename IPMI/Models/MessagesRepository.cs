using IPMI.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IPMI.Models;
namespace IPMI.Models
{
    public class MessagesRepository
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public IEnumerable<Messages> GetAllMessages(string Ke)
        {
            var messages = new List<Messages>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"select count(a.noipmi) as jumlah from tIpmi a 
                                                    inner join tIPMIDetail b on a.noipmi = b.noipmi 
                                                    where b.ke='" + Ke + "' and a.status='open'", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //messages.Add(item: new Messages { MessageID = (int)reader["MessageID"], Message = (string)reader["Message"], EmptyMessage =  reader["EmptyMessage"] != DBNull.Value ? (string) reader["EmptyMessage"] : "", MessageDate = Convert.ToDateTime(reader["Date"]) });
                        messages.Add(item: new Messages { Jumlah = reader["jumlah"].ToString() });
                    }
                }
              
            }
            return messages;
           
            
        }
        public IEnumerable<Messages> GetCompletedTask(string Ke)
        {
            var messages = new List<Messages>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"select count(a.noipmi) as jumlah from tIpmi a 
                                                    inner join tIPMIDetail b on a.noipmi = b.noipmi 
                                                    where b.ke='" + Ke + "' and a.status='closed'", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //messages.Add(item: new Messages { MessageID = (int)reader["MessageID"], Message = (string)reader["Message"], EmptyMessage =  reader["EmptyMessage"] != DBNull.Value ? (string) reader["EmptyMessage"] : "", MessageDate = Convert.ToDateTime(reader["Date"]) });
                        messages.Add(item: new Messages { Jumlah = reader["jumlah"].ToString() });
                    }
                }

            }
            return messages;


        }

        public IEnumerable<Messages> GetCreatedTask(string Ke)
        {
            var messages = new List<Messages>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"select count(*) as jumlah from tIpmi where dari='" + Ke + "'", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //messages.Add(item: new Messages { MessageID = (int)reader["MessageID"], Message = (string)reader["Message"], EmptyMessage =  reader["EmptyMessage"] != DBNull.Value ? (string) reader["EmptyMessage"] : "", MessageDate = Convert.ToDateTime(reader["Date"]) });
                        messages.Add(item: new Messages { Jumlah = reader["jumlah"].ToString() });
                    }
                }

            }
            return messages;


        }

        public IEnumerable<Messages> GetCreatedTaskCompleted(string Ke)
        {
            var messages = new List<Messages>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"select count(*) as jumlah from tIpmi where dari='" + Ke + "' and status='Closed'", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //messages.Add(item: new Messages { MessageID = (int)reader["MessageID"], Message = (string)reader["Message"], EmptyMessage =  reader["EmptyMessage"] != DBNull.Value ? (string) reader["EmptyMessage"] : "", MessageDate = Convert.ToDateTime(reader["Date"]) });
                        messages.Add(item: new Messages { Jumlah = reader["jumlah"].ToString() });
                    }
                }

            }
            return messages;


        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                MessagesHub.SendMessages();
            }
        }
    }
}