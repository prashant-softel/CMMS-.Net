using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.SMB;
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


namespace CMMSAPIs.Repositories.SMB
{
    public class SMBRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        ErrorLog m_errorLog;
        private MYSQLDBHelper sqlDBHelper;

        public SMBRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment iwebhostobj) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(iwebhostobj);

        }


        internal async Task<List<SMBs>> GetSMBlistofPlant(int PlantId)
        {

            string Query = $@"
                    SELECT 
                        m.SCB_id as SMBId,
                        m.SCB_name as SMBname,
                        m.plant_id as PlantId                  
                    FROM 
                        cms_smb_list m 
                    WHERE 
                        m.plant_id = {PlantId} ;
                ";

            var data = await Context.GetData<SMBs>(Query).ConfigureAwait(false);

            return data;
        }
            
        internal async Task<List<SMBDetails>> GetSMBDataByDate(int SMBId, string Date)
        {

            string query = $@"
            SELECT 
                    m.smb_id AS SMBId,
                    l.SCB_name AS SMBName,
                    l.Inverter_id AS invId,
                    l.Inverter_name AS invName,
                    m.today_date AS DateTime,  
                    m.smb_power AS SMB_temprature,
                    m.smb_voltage AS SMB_voltage,
                    m.smb_total_current AS SMB_current_total,
                    m.grid_status AS SMB_status                FROM 
                    cms_SMB_daily m
                LEFT JOIN cms_smb_list l on l.SCB_id = m.SMB_id

            WHERE 
                m.smb_id = {SMBId} AND DATE(m.today_date) = '{Date}'";

            var data = await Context.GetData<SMBDetails>(query).ConfigureAwait(false);

            return data;
        }

       
       
    }
}
