using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.MFM;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.X500;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Models.Dashboard;
using System.Collections;
using CMMSAPIs.Repositories.Dashboard;


namespace CMMSAPIs.Repositories.MFM
{
    public class MFMRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        ErrorLog m_errorLog;
        private MYSQLDBHelper sqlDBHelper;

        public MFMRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment iwebhostobj) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(iwebhostobj);

        }


        internal async Task<List<MFMDetails>> GetMFMlistofPlant(int PlantId)
        {

            string Query = $@"
                    SELECT 
                        m.M_id as MFMId,
                        m.M_name as MFMname,
                        m.plant_id as PlantId                  
                    FROM 
                        cms_mfm_list m 
                    WHERE 
                        m.plant_id = {PlantId} ;
                ";

            var data = await Context.GetData<MFMDetails>(Query).ConfigureAwait(false);

            return data;
        }
        
        internal async Task<List<MFMGraphData>> GetMFMGraphData(int MFMId, string Date)
        {

            string query = $@"
            SELECT 
                    m.M_id as MFMId,
                    m.today_date AS DateTime,
                    m.AC_Power AS ActivePower, 
                    m.Reactive_Power AS ReactivePower, 
                    m.Energy_import_today AS TodaysImport, 
                    m.Energy_import_total AS TotalImport, 
                    m.Energy_Export_today AS TodaysExport, 
                    m.Energy_Export_total AS TotalExport
                FROM 
                    cms_mfm_daily m
            WHERE 
                m.M_id = {MFMId} AND DATE(m.today_date) = {Date}";

            var data = await Context.GetData<MFMGraphData>(query).ConfigureAwait(false);

            return data;
        }
       
    }
}
