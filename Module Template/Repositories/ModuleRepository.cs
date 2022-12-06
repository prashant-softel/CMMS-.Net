using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMMSAPIs.Repositories
{
    public class JobRepository : GenericRepository
    {
        public JobRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        
        internal async Task<List<Job>> GetJobList()
        {
            List<Job> _Jobs = new List<Job>();
            _Jobs.Add(new Job { title = "Inveter Breakdown" });
            //string myQuery = "SELECT * FROM Jobs LIMIT 1";
            //List<Job> _Jobs = await Context.GetData<Job>(myQuery).ConfigureAwait(false);
            return _Jobs;
        }
    }
}
