using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CMMSAPIs.Helper.CMMS;

namespace CMMSAPIs.Repositories.CleaningRepository
{

    public class MCRepository : CleaningRepository
    {
        private UtilsRepository _utilsRepo;
        public MCRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            moduleType = (int)cleaningType.ModuleCleaning;
        }

        internal async new Task<List<CMMCEquipmentList>> GetEquipmentList(int facilityId)
        {
            string filter = "";

            if (facilityId > 0)
            {
                filter += $" and facilityId={facilityId} ";
            }

            string invQuery = $"select id as invId, name as invName from assets where categoryId = 2 {filter}";

            List<CMMCEquipmentList> invs = await Context.GetData<CMMCEquipmentList>(invQuery).ConfigureAwait(false);


            string smbQuery = $"select id as smbId, name as smbName , parentId, moduleQuantity from assets where categoryId = 4 {filter}";

            List<CMPlanSMB> smbs = await Context.GetData<CMPlanSMB>(smbQuery).ConfigureAwait(false);

            //List<CMSMB> invSmbs = new List<CMSMB>;

            foreach (CMMCEquipmentList inv in invs)
            {
                foreach (CMPlanSMB smb in smbs)
                {
                    if (inv.invId == smb.parentId)
                    {
                        inv?.smbs.Add(smb);
                    }
                }

            }
            return invs;
        }

        internal async Task<CMDefaultResponse> LinkPermitToVegetation(int task_id, int permit_id, int userId)
        {
            /*
            * Primary Table - PMSchedule
            * Set the required fields in primary table for linling permit to MC
            * Code goes here
           */
            //string scheduleQuery = "SELECT PM_Schedule_date as schedule_date, Facility_id as facility_id, Asset_id as asset_id, PM_Frequecy_id as frequency_id " +
            //                        $"FROM pm_task WHERE id = {task_id};";
            //List<ScheduleIDData> scheduleData = await Context.GetData<ScheduleIDData>(scheduleQuery).ConfigureAwait(false);

            CMDefaultResponse response;
            string statusQry = $"SELECT status,ifnull(assignedTo,0) assigned_to, ifnull(ptw_id,0) ptw_id FROM Cleaning_execution WHERE id = {task_id}";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            int assigned_to = Convert.ToInt32(dt1.Rows[0][1]);
            int ptw_id = Convert.ToInt32(dt1.Rows[0][2]);
            if (assigned_to <= 0)
            {
                return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "PM Task is Not Assigned.");
            }

            if (ptw_id > 0)
            {
                return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "PM Task is already Linked to Module Cleaning");
            }


            string permitQuery = "SELECT ptw.id as ptw_id, ptw.code as ptw_code, ptw.title as ptw_title, ptw.status as ptw_status " +
                                    "FROM " +
                                        "permits as ptw " +
                                    $"WHERE ptw.id = {permit_id} ;";
            List<ScheduleLinkedPermit> permit = await Context.GetData<ScheduleLinkedPermit>(permitQuery).ConfigureAwait(false);
            if (permit.Count == 0)
                return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, $"Permit {permit_id} does not exist.");
            string myQuery = "UPDATE cleaning_execution  SET " +
                                $"ptw_id = {permit[0].ptw_id}, " +
                                $"status = {(int)CMMS.CMMS_Status.MC_LINKED_TO_PTW}, " +
                                $"updatedAt = '{UtilsRepository.GetUTCTime()}', " +
                                $"updatedById = '{userId}' " +
                                $"WHERE id = {task_id};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.MC_PLAN, task_id, CMMS.CMMS_Modules.PTW, permit_id, "PTW linked to MC", CMMS.CMMS_Status.MC_LINKED_TO_PTW, userId);

            response = new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.SUCCESS, $"Permit {permit_id} linked to Task {task_id}");
            return response;
        }
    }
}
