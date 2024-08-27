using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CMMSAPIs.Helper
{
    public class SchedulerService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private System.Threading.Timer _timerNotification;
        public IConfiguration _iconfiguration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public SchedulerService(IServiceScopeFactory serviceScopeFactory, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IConfiguration iconfiguration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _env = env;
            _iconfiguration = iconfiguration;
        }
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timerNotification = new Timer(RunJob, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(1)); /*Set Interval time here*/
            Schedule_InformationLog("From Scheduler Service : Scheduler started at :- " + DateTime.Now);
            return Task.CompletedTask;
        }
        private void RunJob(object state)
        {
            using (var scrope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    string msg = "From Scheduler Service : Sechduler run at : " + DateTime.Now;
                    DateTime datetimenow = DateTime.Now;
                    var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    string dailyTime = MyConfig.GetValue<string>("Timer:EscalateTime");
                    //daily mail
                    msg = "From Scheduler Service : Current Time : " + DateTime.Now;
                    Schedule_InformationLog(msg);
                    msg = "From Scheduler Service : Escalate Scheduled Mail Sending Time : " + dailyTime;
                    Schedule_InformationLog(msg);

                    //if (DateTime.Now.ToString("HH:mm") == dailyTime)
                    //{
                        Schedule_InformationLog("From Scheduler Service : For Escalate Mail Send : Inside if where dailytime =" + dailyTime);
                        Schedule_InformationLog("From Scheduler Service : For Escalate Mail Send : Started Mail Send functionality.");
                        EmailFunction();
                        async Task<int> EmailFunction()
                        {
                            Schedule_InformationLog("From Scheduler Service : For Escalate Mail Send : EmailFunction() Called from scheduler");
                            string hostName = MyConfig.GetValue<string>("Timer:hostName");
                            bool EscalateSuccess = false;
                            try
                            {
                                string apiUrl = hostName + "/api/EM/Escalate?facilityId=1";
                                Schedule_InformationLog("From Scheduler Service : For Escalate Mail Send Solar : API URL " + apiUrl);
                                CallAPI(apiUrl);
                                EscalateSuccess = true;
                                Schedule_InformationLog("From Scheduler Service : For Daily Mail Send Escalate : Inside try  Daily mail send");
                            }
                            catch (Exception e)
                            {
                                string msg = e.Message;
                                Schedule_ErrorLog("From Scheduler Service : For Daily Mail Send  : Inside catch  Daily mail failed" + msg);
                            }
                            if (EscalateSuccess)
                            {
                                Schedule_InformationLog("From Scheduler Service : For Daily Mail Send :  Mail Sent" + msg);
                                return 1;
                            }
                            else
                            {
                                if (!(EscalateSuccess))
                                {
                                    Schedule_InformationLog("From Scheduler Service : For Daily Mail Send :  Mail send Failed ");
                                }
                                else
                                {
                                    Schedule_InformationLog("From Scheduler Service : For Daily Mail Send :  Mail send Successful.");
                                }
                                return 0;
                            }
                        }
                    //}
                }
                catch (Exception ex)
                {
                    string msg = ex.Message + " exception at :-  " + DateTime.Now;
                    Schedule_ErrorLog(msg);
                }
                Interlocked.Increment(ref executionCount);
            }
        }
        public void CallAPI(string apiUrl)
        {
            try
            {
                Uri address = new Uri(apiUrl);
                Schedule_InformationLog("From Scheduler Service : Inside CallAPI function : Api Url :" + apiUrl);
                // Create the web request
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                // Set type to POST
                request.Method = "POST";
                request.ContentType = "text/xml";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    // Console application output
                    string strOutputXml = reader.ReadToEnd();
                    Schedule_InformationLog("From Scheduler Service : Inside CallAPI function : " + reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                string msg = e.Message;
                Schedule_ErrorLog("inside callApi Function: " + msg);
            }
        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timerNotification?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timerNotification?.Dispose();
        }
        //Log section 
        private void Schedule_ErrorLog(string Message)
        {
            try
            {
                System.IO.File.AppendAllText(@"C:\LogFile\CMMS_Schedule_Log.txt", "**Error**:" + Message + "\r\n");
            }
            catch (Exception e)
            {
            }
            //Read variable from appsetting to enable disable log
        }
        private void Schedule_InformationLog(string Message)
        {
            //Read variable from appsetting to enable disable log
            try
            {
                System.IO.File.AppendAllText(@"C:\LogFile\PPT_Log.txt", "**Info**:" + Message + "\r\n");
            }
            catch (Exception e)
            {
            }
        }
    }
}
