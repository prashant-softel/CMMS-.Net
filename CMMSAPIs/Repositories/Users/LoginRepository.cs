﻿using CMMSAPIs.Helper;
using CMMSAPIs.Models.Users;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Users
{
    public class LoginRepository : GenericRepository
    {
        //private int approve_status = 0;
        public LoginRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {

        }
        //Login 
        internal async Task<CMUserLogin> GetUserLogin(string username, string password)
        {
            string qry = "";
            qry = "SELECT * FROM `login` where `username`='" + username + "' and `password` ='" + password + "' ;";
            // Console.WriteLine(qry);
            var _UserLogin = await Context.GetData<CMUserLogin>(qry).ConfigureAwait(false);
            return _UserLogin.FirstOrDefault();
            // return _UserLogin;
            //  return await Context.GetData(qry).ConfigureAwait(false);

        }

        //  await Context.GetData<WindDashboardData>(qry).ConfigureAwait(false);
        internal async Task<int> eQry(string qry)
        {
            return await Context.ExecuteNonQry<int>(qry).ConfigureAwait(false);

        }


    }

}
