using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Repositories.NewFolder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.ReportBS
{
    public interface ReportBS
    {
        Task<List<MISSUMMARY>> GetMisSummary(string year, int facility_id);
        Task<List<EnviromentalSummary>> GeEnvironmentalSummary(string year, int facility_id);
    }
    public class IReportBS : ReportBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public IReportBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<EnviromentalSummary>> GeEnvironmentalSummary(string year, int facility_id)
        {
            try
            {
                using (var repos = new MISReportRepository(getDB))
                {
                    return await repos.GeEnvironmentalSummary(year, facility_id);

                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<MISSUMMARY>> GetMisSummary(string year, int facility_id)
        {
            try
            {
                using (var repos = new MISReportRepository(getDB))
                {
                    return await repos.GetMisSummary(year, facility_id);

                }
            }
            catch
            {
                throw;
            }
        }
    }
}
