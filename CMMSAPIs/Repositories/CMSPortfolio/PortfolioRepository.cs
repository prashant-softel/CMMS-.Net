using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.Portfolio;
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

namespace CMMSAPIs.Repositories.Portfolio
{
    public class PortfolioRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        ErrorLog m_errorLog;
        private MYSQLDBHelper sqlDBHelper;

        public PortfolioRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment iwebhostobj) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(iwebhostobj);

        }

        internal async Task<AllPlantsEnergy> GetUserPlantsEnergyDetails(int UserId, string Date)
        {

            if (UserId <= 0)
                throw new ArgumentException("Invalid User ID");

            var energyDetailsQry = $@"
                    SELECT 
                        SUM(t.kWh_today_export) AS TodaysEnergy,
                        SUM(t.kWh_total_export) AS TotalEnergy,                       
                        SUM(t.COPR) AS PreventedCO2,                       
                        SUM(t.kWh_today_export) AS ThisMonthEnergy     
                    FROM 
                        userfacilities u
					JOIN 
                        cms_daily_actual_data t ON u.facilityId = t.plant_id  AND 
                        DATE(t.today_date) = '2024-07-27'
                    WHERE 
                        u.userId = {UserId}
					group by u.userId;
                ";

            List<AllPlantsEnergy> energyDetails = await Context.GetData<AllPlantsEnergy>(energyDetailsQry).ConfigureAwait(false);

            return energyDetails[0];
        }

        internal async Task<List<CMSPlantList>> GetUserPlantsDetail(int UserId)
        {

            if (UserId <= 0)
                throw new ArgumentException("Invalid User ID");

            var planListQry = $"SELECT " +
               "f.id as PlantId, " +
               "f.Name AS PlantName, " +
               "f.state AS Location, " +
               "f.Status AS PlantStatus, " +
               "pd.today_date, " +
               "pd.dateid, " +
               "pd.poa_iradiance AS PR, " +
               "pd.ghi_iradiance AS Iradiance, " +
               "f.capacity as Capacity, " +
               "pd.kWh_today_export AS TodayEnergy, " +
               "pd.kWh_total_import AS TotalEnergy, " +
               "pd.active_power AS ActivePower, " +
               "pd.PR_weather_curr AS PRDeviation, " +
               "COUNT(i.inverter_id) as ActiveInverters " +
               "FROM userfacilities u " +
               "JOIN facilities f ON u.facilityId = f.id AND f.isBlock = 0 " +
               "LEFT JOIN cms_daily_actual_data pd ON f.id = pd.plant_id " +
               "LEFT JOIN cms_inverters_daily i ON f.id = i.plant_id AND date(i.today_date) = '2024-07-27' " +
              $"WHERE u.userId = {UserId} AND date(pd.today_date) = '2024-07-27' " +
               "GROUP BY f.id;";


            //List<PlantData> plantDetails = await Context.GetData<PlantData>(planListQry).ConfigureAwait(false);

            var plantDetails = (await Context.GetData<PlantData>(planListQry).ConfigureAwait(false))
            .Select(data => new CMSPlantList
            {
                PlantId = data.PlantId,
                PlantName = data.PlantName,
                Location = data.Location,
                PlantStatus = data.PlantStatus,
                Details = new PlantDetails
                {
                    PR = Convert.ToDouble(data.PR),
                    iradiance = Convert.ToDouble(data.Iradiance),
                    Capacity = Convert.ToDouble(data.Capacity),
                    TodayEnergy = Convert.ToDouble(data.TodayEnergy),
                    TotalEnergy = Convert.ToDouble(data.TotalEnergy),
                    ActivePower = Convert.ToDouble(data.ActivePower),
                    PRDeviation = Convert.ToDouble(data.PRDeviation),
                    ActiveInverters = Convert.ToInt32(data.ActiveInverters)
                }
            }).ToList();

            return plantDetails;
        }

        //internal async Task<PlantPerformanceDetails> GetPlantPerformanceDetails(int PlantId,string Date)
        //{

        //    if (PlantId <= 0)
        //        throw new ArgumentException("Invalid Plant ID");

        //    // Query to get plant details
        //    string plantDetailsQuery = $@"
        //            SELECT 
        //                f.id AS PlantId,
        //                f.Name AS PlantName,
        //                pd.kWh_today_export AS TodaysExport,
        //                pd.kWh_today_import AS TodaysImport,
        //                pd.active_power AS ActivePower,
        //                f.capacity AS PeakPower,
        //                f.capacity AS capacity,
        //                pd.poa_iradiance AS TotalIradiance,
        //                pd.ghi_iradiance AS ForeIradiance,
        //                pd.ghi_iradiance AS HMIIradiance,
        //                pd.NPR AS PerformanceRatio,
        //                pd.GA AS GridAvailability,
        //                pd.PA AS PlantAvailability,
        //                pd.AC_CUF as CUF,
        //                pd.NOPR AS EstimatedPR,
        //                pd.CPR AS EstimatedPRMonthly,
        //                pd.PR_weather_curr AS WeatherCorrectedPR,
        //                pd.COPR AS ContractualPR,
        //                pd.NPR AS GuaranteedPRYearly,
        //                COUNT(DISTINCT invT.inverter_id) as TotalInverters, 
        //                COUNT(DISTINCT invA.inverter_id) as ActiveInverters 
        //            FROM 
        //                cms_daily_actual_data pd
        //            JOIN 
        //                facilities f ON pd.plant_id = f.id
        //            LEFT JOIN cms_inverter_list invT ON f.id = invT.plant_id
        //            LEFT JOIN cms_inverters_daily invA ON f.id = invA.plant_id AND date(invA.today_date) = '2024-07-27'
        //            WHERE 
        //                pd.plant_id = {PlantId} AND 
        //                DATE(pd.today_date) = '2024-07-27';
        //        ";

        //    var plant = await Context.GetData<PlantPerformanceDetails>(plantDetailsQuery).ConfigureAwait(false);

        //    if (plant == null)
        //        throw new Exception("No data found for the specified plant and date.");

        //    //// Query to get total inverters for the plant
        //    //string inverterListQuery = @"
        //    //        SELECT COUNT(*) 
        //    //        FROM cms_inverter_list 
        //    //        WHERE plant_id = @PlantId;
        //    //    ";

        //    //plant.TotalInverters = await connection.ExecuteScalarAsync<int>(inverterListQuery, new { PlantId = plantId });

        //    // Query to get active inverters on the specified date
        //    string activeInvertersQuery = $@"
        //            SELECT 
        //                inv.inverter_id AS InvId,
        //                inv.inverter_name AS Invname,
        //                inv.inverter_capacity AS Capacity, 
        //                i.Inv_PR AS PR, 
        //                i.kWh_today AS TodaysEnergy, 
        //                i.Inv_PR AS PRDeviation, 
        //                i.kwh_till_date AS TotalEnergy, 
        //                i.AC_Power AS Yield, 
        //                i.inverter_status AS status
        //            FROM 
        //             cms_inverter_list inv left join cms_inverters_daily i on inv.inverter_id = i.inverter_id and DATE(i.today_date) = '2024-07-27' 
        //            WHERE 
        //                inv.plant_id = {PlantId};
        //        ";

        //    var inverters = await Context.GetData<Inverter>(activeInvertersQuery).ConfigureAwait(false);

        //    plant[0].Inverters = inverters; 

        //    return plant[0];
        //}


        //internal async Task<List<DashboardGraphData>> GetDashboardGraphDataByDate(int PlantId, string Date)
        //{

        //    string query = $@"
        //    SELECT
        //        i.plant_id as PlantId,
        //        i.today_date AS Timestamp,
        //        i.active_power AS ACPower,
        //        i.poa_iradiance AS POA,
        //        i.NPR AS PR,
        //        i.mod_temp AS ModuleTemp
        //    FROM 
        //        cms_daily_actual_data i
        //    WHERE 
        //        i.plant_id = {PlantId}
        //        AND DATE(i.update_date) = {Date}
        //    ORDER BY i.update_date ASC;
        //";

        //    var data = await Context.GetData<DashboardGraphData>(query).ConfigureAwait(false);

        //    return data;
        //}
        
        //internal async Task<List<Inverter>> GetAllInvertersDataByDate(int PlantId, string Date)
        //{

        //    string activeInvertersQuery = $@"
        //            SELECT 
        //                inv.inverter_id AS InvId,
        //                inv.inverter_name AS Invname,
        //                inv.inverter_capacity AS Capacity, 
        //                i.Inv_PR AS PR, 
        //                i.kWh_today AS TodaysEnergy, 
        //                i.Inv_PR AS PRDeviation, 
        //                i.kwh_till_date AS TotalEnergy, 
        //                i.AC_Power AS Yield, 
        //                i.inverter_status AS status
        //            FROM 
        //            cms_inverters_daily i left join cms_inverter_list inv  on inv.inverter_id = i.inverter_id 
        //            WHERE 
        //                inv.plant_id = {PlantId} and DATE(i.today_date) = '2024-07-27' ;
        //        ";

        //    var inverters = await Context.GetData<Inverter>(activeInvertersQuery).ConfigureAwait(false);

        //    return inverters;
        //}
        //internal async Task<List<Inverter>> GetInvertersDataById(int InvIds, string Date)
        //{

        //    string activeInvertersQuery = $@"
        //            SELECT 
        //                inv.inverter_id AS InvId,
        //                inv.inverter_name AS Invname,
        //                inv.inverter_capacity AS Capacity, 
        //                i.Inv_PR AS PR, 
        //                i.kWh_today AS TodaysEnergy, 
        //                i.Inv_PR AS PRDeviation, 
        //                i.kwh_till_date AS TotalEnergy, 
        //                i.AC_Power AS Yield, 
        //                i.inverter_status AS status
        //            FROM 
        //            cms_inverters_daily i left join cms_inverter_list inv  on inv.inverter_id = i.inverter_id 
        //            WHERE 
        //                inv.inverter_id In ({InvIds}) and DATE(i.today_date) = '2024-07-27' ;
        //        ";

        //    var inverters = await Context.GetData<Inverter>(activeInvertersQuery).ConfigureAwait(false);

        //    return inverters;
        //}
    }
}
