using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.WMS;
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


namespace CMMSAPIs.Repositories.WMS
{
    public class WMSRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        ErrorLog m_errorLog;
        private MYSQLDBHelper sqlDBHelper;

        public WMSRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment iwebhostobj) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(iwebhostobj);

        }


        internal async Task<List<WMSDetails>> GetWMSlistofPlant(int PlantId)
        {

            string Query = $@"
                    SELECT 
                        m.wms_id as WMSId,
                        m.wms_name as WMSname,
                        m.plant_id as PlantId                  
                    FROM 
                        cms_wms_list m 
                    WHERE 
                        m.plant_id = {PlantId} ;
                ";

            var data = await Context.GetData<WMSDetails>(Query).ConfigureAwait(false);

            return data;
        }
            
        internal async Task<List<WMSData>> GetWMSDataByDate(int WMSId, string Date)
        {

            string query = $@"
            SELECT 
                    m.wms_id as WMSId,
                    m.today_date AS DateTime,
                    m.global_irradiance AS GHI, 
                    m.poa_irradiance AS POA, 
                    m.ambient_tmp_avg AS ambTemp, 
                    m.module1_tmp_avg AS modTemp, 
                    m.wind_speed AS windSpeed, 
                    m.wind_direction_avg AS windDirection, 
                    m.humidity_avg AS dailyRain
                FROM 
                    cms_WMS_daily m
            WHERE 
                m.wms_id = {WMSId} AND DATE(m.today_date) = '{Date}';";

            var data = await Context.GetData<WMSData>(query).ConfigureAwait(false);

            return data;
        }

        internal async Task<List<WMSGraphData>> GetMinuteWMSGraphData(int WMSId, string Date)
        {

            string query = $@"
            SELECT 
                    m.wms_id as WMSId,
                    m.today_date AS DateTime,
                    m.global_irradiance AS GHI, 
                    m.global_irradiance AS GHIirradiance, 
                    m.poa_irradiance AS POAirradiance, 
                    m.poa_irradiance AS batteryVoltage
                   
                FROM 
                    cms_WMS_daily m
            WHERE 
                m.wms_id = {WMSId} AND DATE(m.today_date) = '{Date}';";

            var data = await Context.GetData<WMSGraphData>(query).ConfigureAwait(false);

            return data;
        }
        internal async Task<List<WMSGraphData>> GetDailyWMSGraphData(int WMSId, string startDate, string endDate)
        {

            string query = $@"
            SELECT 
                    m.wms_id as WMSId,
                    m.today_date AS DateTime,
                    m.global_irradiance AS GHI, 
                    m.global_irradiance AS GHIirradiance, 
                    m.poa_irradiance AS POAirradiance, 
                    m.poa_irradiance AS batteryVoltage 
                   
                FROM 
                    cms_WMS_daily m
            WHERE 
                m.wms_id = {WMSId} AND DATE(m.today_date) between '{startDate}' and '{endDate}' GROUP BY DATE(today_date); ";

            var data = await Context.GetData<WMSGraphData>(query).ConfigureAwait(false);

            return data;
        }
        internal async Task<List<WMSGraphData>> GetMonthlyWMSGraphData(int WMSId, string startDate, string endDate)
        {

            string query = $@"
            SELECT 
                    m.wms_id as WMSId,
                    DATE_FORMAT(m.today_date, '%M') AS Month,
                    m.today_date AS DateTime,
                    sum(m.global_irradiance) AS GHI, 
                    sum(m.global_irradiance) AS GHIirradiance, 
                    sum(m.poa_irradiance) AS POAirradiance, 
                    avg(m.poa_irradiance) AS batteryVoltage
                   
                FROM 
                    cms_WMS_daily m
            WHERE 
                m.wms_id = {WMSId} AND DATE(m.today_date) between '{startDate}' and '{endDate}' GROUP BY MONTH(today_date);";

            var data = await Context.GetData<WMSGraphData>(query).ConfigureAwait(false);

            return data;
        }
       
       
    }
}
