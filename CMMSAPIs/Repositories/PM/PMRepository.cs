using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.PM
{
    public class PMRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public PMRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        internal async Task<List<CMScheduleData>> GetScheduleData(int facility_id, int? category_id)
        {
            /*
             * Primary Table - PMSchedule
             * Read All properties mention in model and return list
             * Code goes here
            */
            string myQuery = "SELECT Asset_id as asset_id, Asset_Name as asset_name, Asset_Category_id as category_id, Asset_Category_name as category_name from pm_schedule ";
            if (facility_id > 0)
            {
                myQuery += " WHERE Facility_id = " + facility_id;
                if (category_id != null)
                {
                    myQuery += " AND  Asset_Category_id = " + category_id;
                }
            }
            else
            {
                throw new ArgumentException("Invalid Facility ID");
            }
            myQuery += " GROUP BY Asset_id ORDER BY Asset_Category_id ASC;";
            List<CMScheduleData> _scheduleList = await Context.GetData<CMScheduleData>(myQuery).ConfigureAwait(false);
            foreach(CMScheduleData schedule in _scheduleList)
            {
                string query2 = "SELECT a.id as schedule_id, a.PM_Frequecy_id as frequency_id, a.PM_Frequecy_Name as frequency_name, " +
                                    "a.PM_Schedule_date as schedule_date FROM pm_schedule as a WHERE a.PM_Schedule_date = (SELECT MAX(b.PM_Schedule_date) " +
                                    "FROM pm_schedule as b WHERE a.Asset_id = b.Asset_id AND a.PM_Frequecy_id = b.PM_Frequecy_id AND b.status NOT IN " +
                                    $"({(int)CMMS.CMMS_Status.PM_REJECT}, {(int)CMMS.CMMS_Status.PM_PTW_TIMEOUT}, {(int)CMMS.CMMS_Status.PM_CANCELLED}) AND " +
                                    $"PM_Rescheduled = 0) AND a.Asset_id = {schedule.asset_id} GROUP BY PM_Frequecy_id ORDER BY PM_Frequecy_id;";
                List<ScheduleFrequencyData> _freqData = await Context.GetData<ScheduleFrequencyData>(query2).ConfigureAwait(false);
                schedule.frequency_dates = _freqData;
            }
            return _scheduleList;
        }

        internal async Task<List<CMDefaultResponse>> SetScheduleData(CMSetScheduleData request, int userID)
        {
            /*
             * Primary Table - PMSchedule
             * Set All properties mention in model and return list
             * Code goes here
            */
            List<CMDefaultResponse> responseList = new List<CMDefaultResponse>();
            string myQuery1 = $"SELECT id, CONCAT(firstName,' ',lastName) as full_name, loginId as user_name, mobileNumber as contact_no " +
                                        $"FROM users WHERE id = {userID};";
            List<CMUser> user = await Context.GetData<CMUser>(myQuery1).ConfigureAwait(false);
            string myQuery2 = $"SELECT id, name, address, city, state, country, zipcode as pin FROM facilities WHERE id = {request.facility_id};";
            List<CMFacility> facility = await Context.GetData<CMFacility>(myQuery2).ConfigureAwait(false);
            foreach(var asset_schedule in request.asset_schedules)
            {
                CMDefaultResponse response = null;
                string myQuery3 = $"SELECT id, facilityId, categoryId, name FROM assets WHERE id = {asset_schedule.asset_id};";
                List<CMAsset> asset = await Context.GetData<CMAsset>(myQuery3).ConfigureAwait(false);
                if (asset[0].facilityId != request.facility_id)
                {
                    response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.INVALID_ARG, $"Facility ID {request.facility_id} does not include Asset ID {asset_schedule.asset_id}");
                    responseList.Add(response);
                    continue;
                }
                string myQuery4 = $"SELECT serialNumber FROM assets WHERE id = {asset_schedule.asset_id};";
                DataTable dt = await Context.FetchData(myQuery4).ConfigureAwait(false);
                string serialNumber = Convert.ToString(dt.Rows[0][0]);
                string myQuery5 = $"SELECT id, name, description FROM assetcategories WHERE id = {asset[0].categoryId};";
                List<CMAssetCategory> category = await Context.GetData<CMAssetCategory>(myQuery5).ConfigureAwait(false);
                if (category.Count == 0)
                {
                    CMAssetCategory cat = new CMAssetCategory();
                    cat.id = 0;
                    cat.name = "Others";
                    cat.description = "Others";
                    category.Add(cat);
                }
                foreach(var frequency_schedule in asset_schedule.frequency_dates)
                {
                    string myQuery6 = $"SELECT * FROM frequency WHERE id = {frequency_schedule.frequency_id};";
                    List<CMFrequency> frequency = await Context.GetData<CMFrequency>(myQuery6).ConfigureAwait(false);
                    string myQuery7 = "SELECT id as schedule_id, PM_Schedule_date as schedule_date, Facility_id as facility_id, Asset_id as asset_id, PM_Frequecy_id as frequency_id " +
                                        $"FROM pm_schedule WHERE Asset_id = {asset_schedule.asset_id} AND PM_Frequecy_id = {frequency_schedule.frequency_id} " +
                                        $"AND status NOT IN ({(int)CMMS.CMMS_Status.PM_CANCELLED}, {(int)CMMS.CMMS_Status.PM_REJECT}) AND PM_Rescheduled = 0;";
                    List<ScheduleIDData> scheduleData = await Context.GetData<ScheduleIDData>(myQuery7).ConfigureAwait(false);
                    if(scheduleData.Count > 0)
                    {
                        if(frequency_schedule.schedule_date != null)
                        {
                            string updateQry = $"UPDATE pm_schedule SET PM_Schedule_date = '{((DateTime)frequency_schedule.schedule_date).ToString("yyyy'-'MM'-'dd")}', " +
                                                $"PM_Schedule_updated_by = {userID}, PM_Schedule_updated_date = '{UtilsRepository.GetUTCTime()}' " + 
                                                $"WHERE id = {scheduleData[0].schedule_id};";
                            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, scheduleData[0].schedule_id, 0, 0, "PM Schedule Details Updated", CMMS.CMMS_Status.PM_UPDATE, userID);
                            response = new CMDefaultResponse(scheduleData[0].schedule_id, CMMS.RETRUNSTATUS.SUCCESS, "Schedule date updated successfully");
                            responseList.Add(response);
                        }
                        else
                        {
                            string deleteQry = $"DELETE FROM pm_schedule WHERE id = {scheduleData[0].schedule_id};";
                            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, scheduleData[0].schedule_id, 0, 0, "PM Schedule Deleted", CMMS.CMMS_Status.PM_DELETED, userID);
                            response = new CMDefaultResponse(scheduleData[0].schedule_id, CMMS.RETRUNSTATUS.SUCCESS, "Schedule data deleted due to empty date");
                            responseList.Add(response);
                        }
                    }
                    else
                    {
                        if(frequency_schedule.schedule_date != null)
                        {
                            string mainQuery = $"INSERT INTO pm_schedule(PM_Schedule_Date, PM_Frequecy_Name, PM_Frequecy_id, PM_Frequecy_Code, " +
                               $"Facility_id, Facility_Name, Facility_Code, Asset_Category_id, Asset_Category_Code, Asset_Category_name, " +
                               $"Asset_id, Asset_Code, Asset_Name, PM_Schedule_User_id, PM_Schedule_User_Name, PM_Schedule_Emp_id, PM_Schedule_Emp_name, " +
                               $"PM_Schedule_created_date, Asset_Sno, status) VALUES " +
                               $"('{((DateTime)frequency_schedule.schedule_date).ToString("yyyy'-'MM'-'dd")}', '{frequency[0].name}', {frequency[0].id}, 'FRC{frequency[0].id}', " +
                               $"{facility[0].id}, '{facility[0].name}', 'FAC{facility[0].id + 1000}', {category[0].id}, 'AC{category[0].id + 1000}', '{category[0].name}', " +
                               $"{asset[0].id}, 'INV{asset[0].id}', '{asset[0].name}', {user[0].id}, '{user[0].full_name}', {user[0].id}, '{user[0].full_name}', " +
                               $"'{UtilsRepository.GetUTCTime()}', '{serialNumber}', {(int)CMMS.CMMS_Status.PM_SUBMIT}); SELECT LAST_INSERT_ID();";
                            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
                            int id = Convert.ToInt32(dt2.Rows[0][0]);
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, id, 0, 0, "PM Schedule Created", CMMS.CMMS_Status.PM_SUBMIT, userID);
                            response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "PM Schedule data inserted successfully");
                            responseList.Add(response);
                        }
                        else
                        {
                            response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.SUCCESS, "Date cannot be null");
                            responseList.Add(response);
                        }
                    }
                }
            }

            string setCodeNameQuery = "UPDATE pm_schedule " +
                                        "SET PM_Schedule_Code = CONCAT(id,Facility_Code,Asset_Category_Code,Asset_Code,PM_Frequecy_Code), " +
                                        "PM_Schedule_Name = CONCAT(id,' ',Facility_Name,' ',Asset_Category_name,' ',Asset_Name), " +
                                        "PM_Schedule_Number = CONCAT('SCH',id), " +
                                        "PM_Maintenance_Order_Number = CONCAT('PMSCH',id);";
            await Context.ExecuteNonQry<int>(setCodeNameQuery);
            return responseList;
        }
    }
}
