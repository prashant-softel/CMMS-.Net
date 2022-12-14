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
        Task<List<CMIncidentList>> GetIncidentList(int facility_id, DateTime start_date, DateTime end_date);
        Task<CMDefaultResponse> CreateIncidentReport(CMCreateIncidentReport request);
        Task<CMViewIncidentReport> ViewIncidentReport(int id);
        Task<CMDefaultResponse> UpdateIncidentReport(CMCreateIncidentReport request);
        Task<CMDefaultResponse> ApproveIncidentReport(int id);
        Task<CMDefaultResponse> RejectIncidentReport(int id);

    }
    public class IncidentReportBS : IIncidentReportBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public IncidentReportBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMIncidentList>> GetIncidentList(int facility_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.GetIncidentList(facility_id, start_date, end_date);

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

        public async Task<CMDefaultResponse> UpdateIncidentReport(CMCreateIncidentReport request)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.UpdateIncidentReport(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveIncidentReport(int id)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.ApproveIncidentReport(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectIncidentReport(int id)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.RejectIncidentReport(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
