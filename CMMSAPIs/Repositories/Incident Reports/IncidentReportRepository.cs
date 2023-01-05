using CMMSAPIs.Helper;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Incident_Reports
{
    public class IncidentReportRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public IncidentReportRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        internal async Task<List<CMIncidentList>> GetIncidentList(int facility_id, DateTime start_date, DateTime end_date)
        {
            /*
             * Fetch all the CMIncidentList model data from Incidents table and join other table to get string value for facility, user using there respctive tables
             * Update the string value of status from constant defined for incident report
             * Your code goes here
            */
            return null;
        }
        
        internal async Task<CMDefaultResponse> CreateIncidentReport(CMCreateIncidentReport request)
        {
            /*
             * Table - Incidents
             * Need to insert all CMCreateIncidentReport model details in Incidents table
             * Add created log in history table
             * Your code goes here
            */

            //Add Log in history uncomment the below code

            /*CMLog _log = new CMLog();
            _log.module_type = Constant.ROLE_DEFAULT_ACCESS_MODULE;
            _log.module_ref_id = request.role_id;
            _log.comment = JsonSerializer.Serialize(old_access_list);
            _log.status = Constant.UPDATED;
            await _utilsRepo.AddLog(_log);*/
            return null;
        }

        internal async Task<CMViewIncidentReport> ViewIncidentReport(int id)
        {
            /*
             * Fetch all the CMViewIncidentReport model data from Incidents table
             * User - Users
             * Facility & Block - Facility
             * Equipment - Assets
             * status - define in constants file
             * 
             * Also fetch the history records from history table for this particular id
             * 
             * Your code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> UpdateIncidentReport(CMCreateIncidentReport request)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> ApproveIncidentReport(int id)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> RejectIncidentReport(int id)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here
            */
            return null;
        }


    }
}
