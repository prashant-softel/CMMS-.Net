using CMMSAPIs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using CMMSAPIs.Repositories;
using CMMSAPIs.Controllers;
using System.Net;
using System.IO;
using System.Collections.Generic;
using CMMSAPIs.Models.EscalationMatrix;
using CMMSAPIs.Repositories.EscalationMatrix;

namespace CMMSAPIs.Helper
{
    public class SchedulerService : IHostedService, IDisposable
    {
        private int executionCount = 0;

        private System.Threading.Timer _timerNotification;
        public IConfiguration _iconfiguration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        //private MYSQLDBHelper getDB => databaseProvider.SqlInstance();

        public SchedulerService(IServiceScopeFactory serviceScopeFactory, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IConfiguration iconfiguration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _env = env;
            _iconfiguration = iconfiguration;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {

            _timerNotification = new Timer(RunJob, null, TimeSpan.Zero,
              TimeSpan.FromMinutes(5)); /*Set Interval time here*/
            return Task.CompletedTask;
        }
        

        private async void RunJob(object state)
        {

            using (var scrope = _serviceScopeFactory.CreateScope())
            {
                try
                {                    
                    string msg = "Sechduler run at : " + DateTime.Now;                
                    var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    string connectionstring = MyConfig.GetValue<string>("ConnectionStrings:Con");
                    MYSQLDBHelper db = new MYSQLDBHelper(connectionstring);

                

                    var repo = new EscalationMatrixRepository(db);
                    //repo.getEscalationLevel();

                }


                catch (Exception ex)
                {
                    string msg = ex.Message + " exception at :-  " + DateTime.Now;                 
                }
                Interlocked.Increment(ref executionCount);
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

    }
}
