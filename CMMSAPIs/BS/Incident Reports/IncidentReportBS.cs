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
        Task<CMDefaultResponse> CreateIncidentReport(CMCreateIncidentReport request,int userId);
        Task<CMDefaultResponse> UpdateIncidentInvestigationReport(CMCreateIncidentReport request,int userId);
        Task<CMViewIncidentReport> GetIncidentDetailsReport(int id);
        Task<CMDefaultResponse> UpdateIncidentReport(CMCreateIncidentReport request, int userId);
        Task<CMDefaultResponse> ApproveIncidentReport(CMApproveIncident request, int userId);
        Task<CMDefaultResponse> RejectIncidentReport(CMApproveIncident  request, int userId);
        Task<CMDefaultResponse> ApproveCreateIR(CMApproveIncident request, int userId);
        Task<CMDefaultResponse> RejectCreateIR(CMApproveIncident request, int userId);
        Task<CMDefaultResponse> CancelIR(CMApproveIncident request, int userId);

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

        public async Task<CMDefaultResponse> CreateIncidentReport(CMCreateIncidentReport request,int userId)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.CreateIncidentReport(request,userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMDefaultResponse> UpdateIncidentInvestigationReport(CMCreateIncidentReport request, int userId)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.UpdateIncidentInvestigationReport(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMViewIncidentReport>GetIncidentDetailsReport(int id)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.GetIncidentDetailsReport(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMDefaultResponse> UpdateIncidentReport(CMCreateIncidentReport request, int userId)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.UpdateIncidentReport(request,userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveIncidentReport(CMApproveIncident request, int userId)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.ApproveIncidentReport(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectIncidentReport(CMApproveIncident request, int userId)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.RejectIncidentReport(request,userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveCreateIR(CMApproveIncident request, int userId)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.ApproveCreateIR(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectCreateIR(CMApproveIncident request, int userId)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.RejectCreateIR(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CancelIR(CMApproveIncident request, int userId)
        {
            try
            {
                using (var repos = new IncidentReportRepository(getDB))
                {
                    return await repos.CancelIR(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
