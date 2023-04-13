using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.EscalationMatrix;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CMMSAPIs.Models.PM;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CMMSAPIs.Repositories.EscalationMatrix
{
    public class EscalationMatrixRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public EscalationMatrixRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        internal async Task<CMDefaultResponse> InsertEscalationMatrixData(EscalationMatrixModel request, int userID)
        {
            /*
             * Primary Table - PMSchedule
             * Set All properties mention in model and return list
             * Code goes here
            */
            string mainQuery = $"INSERT INTO escalationmatrix" +
                               $"(Module, Status, EscalationLevel, NoOfDay," +
                               $"createdBy, CreatedAt, updatedBy," +
                               $"updatedAt, isActive, isDone)" +
                               $"VALUES" +
                                $"('{request.Module}', '{request.Status}', {request.EscalationLevelList[0].Levels}, '{request.NoOfDayList[0].NoOfDays}', " +
                                $"{userID}, '{UtilsRepository.GetUTCTime()}', null, " +
                                $"null, 1, 0); select 1 Dummay;";

            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            //await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, id, 0, 0, "PM Schedule Created", CMMS.CMMS_Status.CREATED, userID);
            CMDefaultResponse response = new CMDefaultResponse(4, CMMS.RETRUNSTATUS.SUCCESS, "Data saved successfully.");
            return response;
        }
    }
}
