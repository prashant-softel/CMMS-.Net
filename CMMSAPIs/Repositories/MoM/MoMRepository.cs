using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CMMSAPIs.BS.MoM;
using CMMSAPIs.Helper;
using CMMSAPIs.Models;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.MoM;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.Permits;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.WC;
using CMMSAPIs.Repositories.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities.Collections;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using static iTextSharp.text.pdf.AcroFields;


namespace CMMSAPIs.Repositories.MoM
{
    public class MoMRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public MoMRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }
        internal async Task<List<CMMoM>> GetMoMList(int facility_id, string facilitytime)
        {
            string myQuery = $"SELECT distinct   mom_master.id ,Issue , open_date as openDate , close_date as closeDate , mom_master.target_date as targetDate, mom_master.assign_to , mom_master.status , action_plan ," +
                $"  CONCAT(users.firstName , ' ' , users.lastName) as AddedBy, mom_master.added_at as AddedAt , updated_by , updated_at " +
                $" FROM  mom_master   " +
                $" left join  mom_assignto on mom_assignto.mom_id = mom_master.id" +
                $" left join mom_facilities on mom_facilities.mom_id = mom_master.id " +
                $" left join mom_targetdate on mom_targetdate.mom_id = mom_master.id " +
                $" left join users on users.id = mom_assignto.create_by" +
                $" where  is_active = 1; ";

            List<CMMoM> _MoMList = await Context.GetData<CMMoM>(myQuery).ConfigureAwait(false);
            for(var i = 0; i < _MoMList.Count; i++)
            {
                if (_MoMList[i].Status == (int)CMMS.CMMS_Status.MoM_OPEN)
                {
                    _MoMList[i].Status_long = "MoM Open";
                }
                else if (_MoMList[i].Status == (int)CMMS.CMMS_Status.MoM_CLOSE)
                {
                    _MoMList[i].Status_long = "MoM Close";
                }
                else if (_MoMList[i].Status == (int)CMMS.CMMS_Status.MoM_CANCEL)
                {
                    _MoMList[i].Status_long = "MoM Cancel";
                }
            }
    

            return _MoMList;
        }

        internal async Task<CMMoM> GetMoMDetails(int facility_id, int mom_id, string facilitytime)
        {
            string myQuery = $"SELECT distinct   mom_master.id ,Issue , open_date as openDate, close_date as closeDate, mom_master.target_date as targetDate, mom_master.assign_to , mom_master.status , action_plan ," +
                $" added_at,CONCAT(users.firstName , ' ' , users.lastName) as AddedBy, mom_master.added_at as AddedAt , updated_by , updated_at " +
                $" FROM  mom_master   " +
                $" left join  mom_assignto on mom_assignto.mom_id = mom_master.id" +
                $" left join mom_facilities on mom_facilities.mom_id = mom_master.id " +
                $" left join mom_targetdate on mom_targetdate.mom_id = mom_master.id " +
                $" left join users on users.id = mom_assignto.create_by" +
                $" where mom_master.id = {mom_id} and is_active = 1; ";

            List<CMMoM> _MoMList = await Context.GetData<CMMoM>(myQuery).ConfigureAwait(false);
            for (var i = 0; i < _MoMList.Count; i++)
            {
                if (_MoMList[i].Status == (int)CMMS.CMMS_Status.MoM_OPEN)
                {
                    _MoMList[i].Status_long = "MoM Open";
                }
                else if (_MoMList[i].Status == (int)CMMS.CMMS_Status.MoM_CLOSE)
                {
                    _MoMList[i].Status_long = "MoM Close";
                }
                else if (_MoMList[i].Status == (int)CMMS.CMMS_Status.MoM_CANCEL)
                {
                    _MoMList[i].Status_long = "MoM Cancel";
                }
            }

            return _MoMList[0];
        }

        internal async Task<CMDefaultResponse> CreateMoM(CMMoM request, int userId)
        {
            string myQuery = $"INSERT INTO mom_master(Issue, open_date, close_date,target_date,assign_to,status,action_plan, added_by, added_at) VALUES " +
                                $"('{request.Issue}','{request.OpenDate.Value.ToString("yyyy-MM-dd HH:mm")}','{request.CloseDate.Value.ToString("yyyy-MM-dd HH:mm")}','{request.TargetDate.Value.ToString("yyyy-MM-dd HH:mm")}','{request.AssignTo}', {(int)CMMS.CMMS_Status.MoM_OPEN},'{request.ActionPlan}' , {userId}, '{UtilsRepository.GetUTCTime()}'); " +
                                 $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int mom_id = Convert.ToInt32(dt.Rows[0][0]);

            if (request.mom_assignto != null)
            {
                foreach (var assignTo in request.mom_assignto)
                {
                    string insertAssignToQuery = $"INSERT INTO mom_assignto(mom_id, assign_To, created_at, create_by) VALUES " +
                                                    $"({mom_id}," +
                                                    $"{assignTo.assign_to_id}," +
                                                    $"'{UtilsRepository.GetUTCTime()}'," +
                                                    $"{userId});";
                    await Context.ExecuteNonQry<int>(insertAssignToQuery).ConfigureAwait(false);
                }
            }

            if (request.mom_target_date != null)
            {
                foreach (var item in request.mom_target_date)
                {
                    string insertQuery = $"INSERT INTO mom_targetdate(mom_id, target_date, created_at, created_by) VALUES " +
                                                    $"({mom_id}," +
                                                    $"'{item.target_date.Value.ToString("yyyy-MM-dd HH:mm")}'," +
                                                    $"'{UtilsRepository.GetUTCTime()}'," +
                                                    $"{userId});";
                    await Context.ExecuteNonQry<int>(insertQuery).ConfigureAwait(false);
                }
            }

            if (request.mom_facilities != null)
            {
                foreach (var item in request.mom_facilities)
                {
                    string insertQuery = $"INSERT INTO mom_facilities(mom_id, facility_Id, created_at, created_by) VALUES " +
                                                    $"({mom_id}," +
                                                    $"{item.facility_id}," +
                                                    $"'{UtilsRepository.GetUTCTime()}'," +
                                                    $"{userId});";
                    await Context.ExecuteNonQry<int>(insertQuery).ConfigureAwait(false);
                }
            }
            return new CMDefaultResponse(mom_id, CMMS.RETRUNSTATUS.SUCCESS, "MoM created.");
        }
        internal async Task<CMDefaultResponse> UpdateMoM(CMMoM request, int userId)
        {
            string myQuery = $"UPDATE mom_master SET " +
                             $"Issue = '{request.Issue}', " +
                             $"open_date = '{request.OpenDate.Value.ToString("yyyy-MM-dd HH:mm")}', " +
                             $"close_date = '{request.CloseDate.Value.ToString("yyyy-MM-dd HH:mm")}', " +
                             $"target_date = '{request.TargetDate.Value.ToString("yyyy-MM-dd HH:mm")}', " +
                             $"assign_to = '{request.AssignTo}', " +
                             $"action_plan = '{request.ActionPlan}', " +
                             $"added_by = {userId}, " +
                             $"added_at = '{UtilsRepository.GetUTCTime()}' " +
                             $"WHERE id = {request.Id};";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);

            int mom_id = request.Id;

            if (request.mom_assignto != null)
            {
                string insertAssignToQuery = "DELETE FROM mom_assignto where mom_id = "+mom_id+"; INSERT INTO mom_assignto(mom_id, assign_To, created_at, create_by) VALUES ";

                List<string> valueList = new List<string>();
                foreach (var assignTo in request.mom_assignto)
                {
                    valueList.Add($"({mom_id}, {assignTo.assign_to_id}, '{UtilsRepository.GetUTCTime()}', {userId})");
                }

                insertAssignToQuery += string.Join(",", valueList);

                await Context.ExecuteNonQry<int>(insertAssignToQuery).ConfigureAwait(false);
            }

            if (request.mom_target_date != null)
            {
                string insertQuery = "DELETE FROM mom_targetdate where mom_id = " + mom_id + "; INSERT INTO mom_targetdate(mom_id, target_date, created_at, created_by) VALUES ";

                List<string> valueList = new List<string>();
                foreach (var item in request.mom_target_date)
                {
                    valueList.Add($"({mom_id}, '{item.target_date.Value.ToString("yyyy-MM-dd HH:mm")}', '{UtilsRepository.GetUTCTime()}', {userId})");
                }

                insertQuery += string.Join(",", valueList);

                await Context.ExecuteNonQry<int>(insertQuery).ConfigureAwait(false);
            }

            if (request.mom_facilities != null)
            {
                string insertQuery = "DELETE FROM mom_facilities where mom_id = " + mom_id + "; INSERT INTO mom_facilities(mom_id, facility_Id, created_at, created_by) VALUES ";

                List<string> valueList = new List<string>();
                foreach (var item in request.mom_facilities)
                {
                    valueList.Add($"({mom_id}, {item.facility_id}, '{UtilsRepository.GetUTCTime()}', {userId})");
                }

                insertQuery += string.Join(",", valueList);

                await Context.ExecuteNonQry<int>(insertQuery).ConfigureAwait(false);
            }

            return new CMDefaultResponse(request.Id, CMMS.RETRUNSTATUS.SUCCESS, "MoM updated.");
        }

        internal async Task<CMDefaultResponse> DeleteMoM(int momId)
        {
            string myQuery = $"UPDATE mom_master SET is_active = 0 WHERE id = {momId};";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            return new CMDefaultResponse(momId, CMMS.RETRUNSTATUS.SUCCESS, "MoM deleted.");
        }

        internal async Task<CMDefaultResponse> CloseMoM(int momId)
        {
            string myQuery = $"UPDATE mom_master SET status = {(int)CMMS.CMMS_Status.MoM_CLOSE} WHERE id = {momId};";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            return new CMDefaultResponse(momId, CMMS.RETRUNSTATUS.SUCCESS, "MoM Close.");
        }
        internal async Task<List<CMMoMAssignTo>> GetMoMAssignToDetails(int mom_id)
        {
            string myQuery = $"select mom_assignto.id,mom_id,assign_To as assign_to_id,   CONCAT(assign.firstName , ' ' , assign.lastName) as assign_to_name," +
                $" CONCAT(users.firstName , ' ' , users.lastName) as AddedBy, created_at as AddedAt " +
                $" from mom_assignto" +
                $" left join users on users.id = mom_assignto.create_by" +
                $" left join users assign on assign.id = mom_assignto.assign_To" +
                $" where mom_id = {mom_id}";

            List<CMMoMAssignTo> _List = await Context.GetData<CMMoMAssignTo>(myQuery).ConfigureAwait(false);

            return _List;
        }
        internal async Task<List<CMMoMTargetDate>> GetMoMTargetDateDetails(int mom_id)
        {
            string myQuery = $"select mom_targetdate.id,mom_id,target_date, " +
                $" CONCAT(users.firstName , ' ' , users.lastName) as AddedBy, created_at as AddedAt " +
                $" from mom_targetdate" +
                $" left join users on users.id = mom_targetdate.created_by" +
                $" where mom_id = {mom_id}";

            List<CMMoMTargetDate> _List = await Context.GetData<CMMoMTargetDate>(myQuery).ConfigureAwait(false);

            return _List;
        }
    }
}
