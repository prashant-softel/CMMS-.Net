using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.Users;
using CMMSAPIs.Models.Users;

namespace CMMSAPIs.BS.Users
{
    public interface iLoginBS
    {
        Task<int> eQry(string qry);
        Task<UserLogin> GetUserLogin(string username, string password);

    }
    public class LoginBS : iLoginBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public LoginBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<UserLogin> GetUserLogin(string username, string password)
        {
            try
            {
                using (var repos = new LoginRepository(getDB))
                {
                    return await repos.GetUserLogin(username, password);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> eQry(string qry)
        {
            try
            {
                using (var repos = new LoginRepository(getDB))
                {
                    return await repos.eQry(qry);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
