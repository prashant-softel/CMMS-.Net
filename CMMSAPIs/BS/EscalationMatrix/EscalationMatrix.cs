using CMMSAPIs.Helper;
using CMMSAPIs.Models.EscalationMatrix;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.EscalationMatrix;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.EscalationMatrix
{
    public interface iEM
    {
        Task<CMDefaultResponse> InsertEscalationMatrixData(CMEscalationMatrixModel request, int userID);
    }
    public class EscalationMatrix : iEM
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public EscalationMatrix(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<CMDefaultResponse> InsertEscalationMatrixData(CMEscalationMatrixModel request, int userID)
        {
            try
            {
                using (var repos = new EscalationMatrixRepository(getDB))
                {
                    return await repos.InsertEscalationMatrixData(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
