using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.Dashboard;
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
using System.Globalization;


namespace CMMSAPIs.Repositories.Dashboard
{
    public class DashboardRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        ErrorLog m_errorLog;
        private MYSQLDBHelper sqlDBHelper;

        public DashboardRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment iwebhostobj) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(iwebhostobj);

        }

        public static string ConvertDate(string dateString)
        {
            // Parse the input date string into a DateTime object
            DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Reformat the date as 'dMMyyyy' (day, month, year)
            string result = $"{date.Day}{date.Month:D2}{date.Year}";

            return result;
        }

        public static string ConvertToDateId(int plantId, string dateString)
        {
            // Convert the date string to DateTime
            DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Reformat the date as 'ddMMyyyy'
            string formattedDate = date.ToString("ddMMyyyy");

            // Return the desired format 'PlantId-ddMMyyyy'
            return $"{plantId}-{formattedDate}";
        }

        internal async Task<PlantPerformanceDetails> GetPlantPerformanceDetails(int PlantId,string Date)
        {

            if (PlantId <= 0)
                throw new ArgumentException("Invalid Plant ID");

            // Query to get plant details
            string plantDetailsQuery = $@"
                    SELECT 
                        f.id AS PlantId,
                        f.Name AS PlantName,
                        pd.kWh_today_export AS TodaysExport,
                        pd.kWh_today_import AS TodaysImport,
                        pd.active_power AS ActivePower,
                        f.capacity AS PeakPower,
                        f.capacity AS capacity,
                        pd.poa_iradiance AS TotalIradiance,
                        pd.ghi_iradiance AS ForeIradiance,
                        pd.ghi_iradiance AS HMIIradiance,
                        pd.NPR AS PerformanceRatio,
                        pd.GA AS GridAvailability,
                        pd.PA AS PlantAvailability,
                        pd.AC_CUF as CUF,
                        pd.NOPR AS EstimatedPR,
                        pd.CPR AS EstimatedPRMonthly,
                        pd.PR_weather_curr AS WeatherCorrectedPR,
                        pd.COPR AS ContractualPR,
                        pd.NPR AS GuaranteedPRYearly,
                        COUNT(DISTINCT invT.inverter_id) as TotalInverters, 
                        COUNT(DISTINCT invA.inverter_id) as ActiveInverters 
                    FROM 
                        cms_daily_actual_data pd
                    JOIN 
                        facilities f ON pd.plant_id = f.id
                    LEFT JOIN cms_inverter_list invT ON f.id = invT.plant_id
                    LEFT JOIN cms_inverters_daily invA ON f.id = invA.plant_id AND DATE(invA.today_date) = {Date}
                    WHERE 
                        pd.dateid ='{ConvertToDateId(PlantId, Date)}';
                ";

            var plant = await Context.GetData<PlantPerformanceDetails>(plantDetailsQuery).ConfigureAwait(false);

            if (plant == null)
                throw new Exception("No data found for the specified plant and date.");

            //// Query to get total inverters for the plant
            //string inverterListQuery = @"
            //        SELECT COUNT(*) 
            //        FROM cms_inverter_list 
            //        WHERE plant_id = @PlantId;
            //    ";

            //plant.TotalInverters = await connection.ExecuteScalarAsync<int>(inverterListQuery, new { PlantId = plantId });

            // Query to get active inverters on the specified date
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
                     cms_inverter_list inv left join cms_inverters_daily i on inv.inverter_id = i.inverter_id and DATE(i.today_date) = '2024-07-27'
                    WHERE 
                        inv.plant_id = {PlantId};
                ";

            var inverters = await Context.GetData<InverterDetails>(activeInvertersQuery).ConfigureAwait(false);

            plant[0].Inverters = inverters; 

            return plant[0];
        }


        internal async Task<List<PowerGraphData>> GetPowerGraphDataByMinute(int PlantId, string Date)
        {

            string query = $@"
            SELECT
                i.plant_id as PlantId,
                i.today_date AS Timestamp,
                i.active_power AS ACPower,
                i.poa_iradiance AS POA,
                i.NPR AS PR,
                i.mod_temp AS ModuleTemp
            FROM 
                cms_daily_actual_data i
            WHERE 
               i.dateid = '{ConvertToDateId(PlantId, Date)}'
            ORDER BY i.update_date ASC;
        ";

            var data = await Context.GetData<PowerGraphData>(query).ConfigureAwait(false);

            return data;
        }
        
        internal async Task<List<EnergyGraphData>> GetEnegryGraphDataByMinute(int PlantId, string Date)
        {

            string query = $@"
            SELECT
                d.plant_id as PlantId,
                d.today_date AS Timestamp, 
                d.poa_iradiance as MeasuredEnergy,
                d.poa_iradiance AS MeasuredPOA, 
                e.kWh_estimated as EstimatedEnergy,
                e.PR_estimated AS EstimatedPOA             
            FROM 
                cms_daily_actual_data d
            LEFT JOIN cms_estimation_daily_data e ON d.plant_id = e.plant_id
            WHERE 
                d.dateid ='{ConvertToDateId(PlantId, Date)}'
            ORDER BY d.update_date ASC;
        ";
            

            var data = await Context.GetData<EnergyGraphData>(query).ConfigureAwait(false);

            return data;
        }
        
        internal async Task<List<WeatherGraphData>> GetWeatherDataByMinute(int PlantId, string Date)
        {

            string query = $@"
            SELECT
                i.plant_id as PlantId,
                i.today_date AS Timestamp,
                i.ghi_iradiance AS GHI,
                i.poa_iradiance AS POA,
                i.amb_temp AS Amb_Temp,
                i.mod_temp AS Mod_Temp_1
            FROM 
                cms_daily_actual_data i
            WHERE 
                i.dateid ='{ConvertToDateId(PlantId, Date)}'
            ORDER BY i.update_date ASC;
        ";           
            
            var data = await Context.GetData<WeatherGraphData>(query).ConfigureAwait(false);

            return data;
        }
        


    }
}
