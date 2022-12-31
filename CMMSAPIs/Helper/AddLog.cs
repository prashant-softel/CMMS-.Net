using Microsoft.Extensions.Configuration;
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

            using (StreamWriter writer = new StreamWriter(log_path, true))
            {
                writer.WriteLine(Message);
            }                
        }
    }
}
