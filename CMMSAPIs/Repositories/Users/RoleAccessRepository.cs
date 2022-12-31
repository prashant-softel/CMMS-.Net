using CMMSAPIs.Helper;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace CMMSAPIs.Repositories.Users
{
    public class RoleAccessRepository : GenericRepository
    {
        //private UtilsRepository _utilRepo;
        private LogHelper _utilRepo;
        private MYSQLDBHelper _conn;
        public RoleAccessRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _conn = sqlDBHelper;
            UtilsRepository _utilRepo = new UtilsRepository(sqlDBHelper);            
        }

        internal async Task<List<CMAccess>> GetRoleAccess(int role_id)
        {
            string qry = $"SELECT " +
                            $"`featureId` as feature_id, `add`, `edit`, `delete`, `view`, `issue`, `approve`, `selfView` " +
                         $"FROM " +
                            $"`RoleAccess` " +
                         $"WHERE " +
                            $"roleId = {role_id}";

            List<CMAccess> access_list = await Context.GetData<CMAccess>(qry).ConfigureAwait(false);
            return access_list;
        }

        internal async Task<CMDefaultResponse> SetRoleAccess(CMRoleAccess request)
        {
            //MySqlTransaction transaction = Context.TheConnection.BeginTransaction();
            try
            {                
                // Get previous settings
                List<CMAccess> old_access_list = await GetRoleAccess(request.role_id);

                if (old_access_list != request.access_list)
                {
                    // Delete the previous setting
                    string delete_qry = $" DELETE FROM RoleAccess WHERE RoleId = {request.role_id}";
                    await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                    // Insert the new setting
                    List<string> role_access = new List<string>();

                    foreach (var access in request.access_list)
                    {
                        role_access.Add($"({request.role_id}, {access.feature_id}, {access.add}, {access.edit}, " +
                                        $"{access.delete}, {access.view}, {access.issue}, {access.approve}, {access.selfView})");
                        
                    }
                    string role_access_insert_str = string.Join(',', role_access);

                    string insert_query = $"INSERT INTO RoleAccess" +
                                                $"(roleId, featureId, `add`, `edit`, `view`, `delete`, `issue`, `approve`, `selfView`) " +
                                          $" VALUES {role_access_insert_str}";
                    await Context.GetData<List<int>>(insert_query).ConfigureAwait(false);

                    using (var repos = new UtilsRepository(_conn))
                    {
                        // Add previous setting to log table
                        CMLog _log = new CMLog();
                        _log.module_type = Constant.ROLE_DEFAULT_ACCESS_MODULE;
                        _log.module_ref_id = request.role_id;
                        _log.comment = JsonSerializer.Serialize(old_access_list);
                        _log.status = Constant.UPDATED;
                        await repos.AddLog(_log);
                    }
                }
                //transaction.Commit();
                CMDefaultResponse response = new CMDefaultResponse();
                response.id = request.role_id;
                response.message = "Updated Role Access Successfully";
                return response;
            }
            catch (Exception)
            {
                //transaction.Rollback();
                throw;
            }            
        }
    }
}
