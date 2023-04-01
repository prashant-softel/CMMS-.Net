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
            string myQuery = "SELECT * from pm_schedule ";
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
            myQuery += " ORDER BY id DESC;";
            List<CMScheduleData> _checkList = await Context.GetData<CMScheduleData>(myQuery).ConfigureAwait(false);
            return _checkList;
        }

        internal async Task<CMDefaultResponse> SetScheduleData(CMSetScheduleData request, int userID)
        {
            /*
             * Primary Table - PMSchedule
             * Set All properties mention in model and return list
             * Code goes here
            */
            string myQuery1 = $"SELECT id, facilityId, categoryId, name FROM assets WHERE id = {request.asset_id};";
            List<CMAsset> asset = await Context.GetData<CMAsset>(myQuery1).ConfigureAwait(false);
            string myQuery2 = $"SELECT serialNumber FROM assets WHERE id = {request.asset_id};";
            DataTable dt = await Context.FetchData(myQuery2).ConfigureAwait(false);
            string serialNumber = Convert.ToString(dt.Rows[0][0]);
            string myQuery3 = $"SELECT id, name, description FROM assetcategories WHERE id = {asset[0].categoryId};";
            List<CMAssetCategory> category = await Context.GetData<CMAssetCategory>(myQuery3).ConfigureAwait(false);
            if(category.Count == 0)
            {
                CMAssetCategory cat = new CMAssetCategory();
                cat.id = 0;
                cat.name = "Others";
                cat.description = "Others";
                category.Add(cat);
            }
            string myQuery4 = $"SELECT id, name, address, city, state, country, zipcode as pin FROM facilities WHERE id = {request.facility_id};";
            List<CMFacility> facility = await Context.GetData<CMFacility>(myQuery4).ConfigureAwait(false);
            string myQuery5 = $"SELECT * FROM frequency WHERE id = {request.frequency_id};";
            List<CMFrequency> frequency = await Context.GetData<CMFrequency>(myQuery5).ConfigureAwait(false);
            string myQuery6 = $"SELECT id, CONCAT(firstName,' ',lastName) as full_name, loginId as user_name, mobileNumber as contact_no " +
                                $"FROM users WHERE id = {userID};";
            List<CMUser> user = await Context.GetData<CMUser>(myQuery6).ConfigureAwait(false);
            string mainQuery = $"INSERT INTO pm_schedule(PM_Schedule_Date, PM_Frequecy_Name, PM_Frequecy_id, PM_Frequecy_Code, " +
                                $"Facility_id, Facility_Name, Facility_Code, Asset_Category_id, Asset_Category_Code, Asset_Category_name, " +
                                $"Asset_id, Asset_Code, Asset_Name, PM_Schedule_User_id, PM_Schedule_User_Name, PM_Schedule_Emp_id, PM_Schedule_Emp_name, " +
                                $"PM_Schedule_created_date, Asset_Sno) VALUES " + 
                                $"('{request.schedule_date.ToString("yyyy'-'MM'-'dd")}', '{frequency[0].name}', {frequency[0].id}, 'FRC{frequency[0].id}', " + 
                                $"{facility[0].id}, '{facility[0].name}', 'FAC{facility[0].id+1000}', {category[0].id}, 'AC{category[0].id+1000}', '{category[0].name}', " + 
                                $"{asset[0].id}, 'INV{asset[0].id}', '{asset[0].name}', {user[0].id}, '{user[0].full_name}', {user[0].id}, '{user[0].full_name}', " + 
                                $"'{UtilsRepository.GetUTCTime()}', '{serialNumber}'); SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            string setCodeNameQuery = "UPDATE pm_schedule " +
                                        "SET PM_Schedule_Code = CONCAT(id+1,Facility_Code,Asset_Category_Code,Asset_Code,PM_Frequecy_Code), " +
                                        "PM_Schedule_Name = CONCAT(id+1,' ',Facility_Name,' ',Asset_Category_name,' ',Asset_Name), " +
                                        "PM_Schedule_Number = CONCAT('SCH',id), " +
                                        "PM_Maintenance_Order_Number = CONCAT('PMSCH',id+1) " + 
                                        $"WHERE id = {id};";
            await Context.ExecuteNonQry<int>(setCodeNameQuery);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, id, 0, 0, "PM Schedule Created", CMMS.CMMS_Status.CREATED, userID);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Preventive Maintenance Schedule Created");
            return response;
        }
    }
}
