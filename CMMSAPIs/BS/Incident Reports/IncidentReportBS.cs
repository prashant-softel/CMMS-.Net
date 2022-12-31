using CMMSAPIs.Helper;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Incident_Reports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Incident_Reports
{
    public interface IIncidentReportBS
    {
        Task<List<CMIncidentList>> GetIncidentList(int facility_id);
        Task<CMDefaultResponse> CreateIncidentReport(CMCreateIncidentReport request);
        Task<CMViewIncidentReport> ViewIncidentReport(int id);
        Task<CMDefaultResponse> FeedBackIncidentReport(CMFeedBackIncidentReport request);
        Task<CMDefaultResponse> CloseIncidentReport(int id);

    }
    public class IncidentReportBS : IIncidentReportBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public IncidentReportBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMIncidentList>> GetIncidentList(int facility_id)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.GetIncidentList(facility_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMDefaultResponse> CreateIncidentReport(CMCreateIncidentReport request)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.CreateIncidentReport(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMViewIncidentReport> ViewIncidentReport(int id)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.ViewIncidentReport(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMDefaultResponse> FeedBackIncidentReport(CMFeedBackIncidentReport request)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.FeedBackIncidentReport(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CloseIncidentReport(int id)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.CloseIncidentReport(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
