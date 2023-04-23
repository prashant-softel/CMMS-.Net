using CMMSAPIs.Models.SM;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.IO;

namespace CMMSAPIs.Helper
{
    public class AddLog
    {
        private readonly IConfiguration _configuration;

        public AddLog(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ErrorLog(string Message)
        {
            string log_path = _configuration.GetValue<string>("Logging:LogPath");
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:Con");

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            string logStmt = "INSERT INTO log4netlog (Date,  Message) VALUES('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"','"+Message+"');";       
            cmd.CommandText = logStmt;
            cmd.ExecuteNonQuery();
            conn.Close();
            using (StreamWriter writer = new StreamWriter(log_path, true))
            {
                writer.WriteLine(Message);
            }                
        }
    }
}
