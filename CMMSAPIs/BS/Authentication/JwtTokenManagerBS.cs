using CMMSAPIs.Helper;
using CMMSAPIs.Models.Authentication;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Repositories.Authenticate;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Authentication
{
    public interface IJwtTokenManagerBS
    {
        Task<UserToken> Authenticate(UserCrentialsModel userCrentials);
    }
    public class JwtTokenManagerBS : IJwtTokenManagerBS
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();

        public JwtTokenManagerBS(DatabaseProvider dbProvider, IConfiguration configuration)
        {
            _configuration = configuration;
            databaseProvider = dbProvider;
        }
        public Task<UserToken> Authenticate(UserCrentialsModel userCrentials)
        {
            try
            {
                using (var repos = new JwtTokenManagerRepository(getDB, _configuration))
                {
                    return repos.Authenticate(userCrentials);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
