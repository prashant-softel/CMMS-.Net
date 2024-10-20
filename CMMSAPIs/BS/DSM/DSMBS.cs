﻿using CMMSAPIs.Helper;
using CMMSAPIs.Models.DSM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.DSM;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CMMSAPIs.BS.DSM
{
    public interface IDSMBS
    {
        Task<List<CMDSMData>> getDSMData(string fy, string month, string stateId, string spvId, string siteId, string dsmtype);
        Task<List<DSMTYPE>> getDSMType();
        Task<CMImportFileResponse> importDSMFile(int file_id, int userID);
    }
    public class DSMBS : IDSMBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();

        public static IWebHostEnvironment _environment;

        public DSMBS(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            databaseProvider = dbProvider;
            _environment = environment;

        }

        public async Task<List<CMDSMData>> getDSMData(string fy, string month, string stateId, string spvId, string siteId, string dsmtype)
        {
            try
            {
                using (var repos = new DSMRepository(getDB, _environment))
                {
                    return await repos.getDSMData(fy, month, stateId, spvId, siteId, dsmtype);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMImportFileResponse> importDSMFile(int file_id, int userID)
        {

            try
            {
                using (var repos = new DSMRepository(getDB, _environment))
                {
                    return await repos.importDSMFile(file_id, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<DSMTYPE>> getDSMType()
        {
            try
            {
                using (var repos = new DSMRepository(getDB, _environment))
                {
                    return await repos.getDSMType();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
    //    public async Task<CMEscalationResponse> Escalate(CMMS.CMMS_Modules module, int id)
    //    {
    //        try
    //        {
    //            using (var repos = new DSMRepository(getDB))
    //            {
    //                return await repos.Escalate(module, id);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw;
    //        }
    //    }
    //    public async Task<List<CMEscalationLog>> ShowEscalationLog(CMMS.CMMS_Modules module, int module_ref_id)
    //    {
    //        try
    //        {
    //            using (var repos = new DSMRepository(getDB))
    //            {
    //                return await repos.ShowEscalationLog(module, module_ref_id);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw;
    //        }
    //    }
    //}
}


