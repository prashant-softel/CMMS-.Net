using CMMSAPIs.Helper;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Incident_Reports
{
    public class IncidentReportRepository : GenericRepository
    {
        public IncidentReportRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {

        }

        internal async Task<List<CMIncidentList>> GetIncidentList(int facility_id)
        {
            /*
             * Your code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> CreateIncidentReport(CMCreateIncidentReport request)
        {
            /*
             * Your code goes here
            */
            return null;
        }

        internal async Task<CMViewIncidentReport> ViewIncidentReport(int id)
        {
            /*
             * Your code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> FeedBackIncidentReport(CMFeedBackIncidentReport request)
        {
            /*
             * Your code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> CloseIncidentReport(int id)
        {
            /*
             * Your code goes here
            */
            return null;
        }


    }
}
