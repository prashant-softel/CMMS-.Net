using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.Inverter;
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

namespace CMMSAPIs.Repositories.Inverter
{
    public class InverterRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        ErrorLog m_errorLog;
        private MYSQLDBHelper sqlDBHelper;

        public InverterRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment iwebhostobj) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(iwebhostobj);

        }


        internal async Task<List<InverterDetails>> GetAllInvertersDataByDate(int PlantId, string Date)
        {

            string activeInvertersQuery = $@"
                    SELECT 
                        inv.inverter_id AS InvId,
                        inv.inverter_name AS Invname,
                        inv.inverter_capacity AS Capacity, 
                        i.Inv_PR AS PR, 
                        i.kWh_today AS TodaysEnergy, 
                        i.Inv_PR AS PRDeviation, 
                        i.kwh_till_date AS TotalEnergy, 
                        i.AC_Power AS Yield, 
                        i.inverter_status AS status
                    FROM 
                    cms_inverters_daily i left join cms_inverter_list inv  on inv.inverter_id = i.inverter_id 
                    WHERE 
                        inv.plant_id = {PlantId} and DATE(i.today_date) = '{Date}' ;
                ";

            var inverters = await Context.GetData<InverterDetails>(activeInvertersQuery).ConfigureAwait(false);

            return inverters;
        }
        internal async Task<List<InverterDetails>> GetInvertersDataById(int InvIds, string Date)
        {

            string activeInvertersQuery = $@"
                    SELECT 
                        inv.inverter_id AS InvId,
                        inv.inverter_name AS Invname,
                        inv.inverter_capacity AS Capacity, 
                        i.Inv_PR AS PR, 
                        i.kWh_today AS TodaysEnergy, 
                        i.Inv_PR AS PRDeviation, 
                        i.kwh_till_date AS TotalEnergy, 
                        i.AC_Power AS Yield, 
                        i.inverter_status AS status
                    FROM 
                    cms_inverters_daily i left join cms_inverter_list inv  on inv.inverter_id = i.inverter_id 
                    WHERE 
                        inv.inverter_id In ({InvIds}) and DATE(i.today_date) = '{Date}' ;
                ";

            var inverters = await Context.GetData<InverterDetails>(activeInvertersQuery).ConfigureAwait(false);

            return inverters;
        }

        internal async Task<List<InverterGraphData>> GetInvGraphData(int PlantId, string Date)
        {

            string query = $@"
                    SELECT 
                        inv.inverter_id AS InvId,
                        inv.inverter_name AS Invname,
                        i.Inv_PR AS PR, 
                        i.kWh_today AS Energy, 
                        i.AC_Power AS Yield
                    FROM 
                    cms_inverters_daily i left join cms_inverter_list inv  on inv.inverter_id = i.inverter_id 
                    WHERE 
                        inv.plant_id In ({PlantId}) and DATE(i.today_date) = '{Date}' ;
        ";

            var data = await Context.GetData<InverterGraphData>(query).ConfigureAwait(false);

            return data;
        }
    }
}
