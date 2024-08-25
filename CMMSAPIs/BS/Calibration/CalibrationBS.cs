using CMMSAPIs.Helper;
using CMMSAPIs.Models.Audit;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Audits;
using CMMSAPIs.Repositories.Calibration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Calibration
{
    public interface ICalibrationBS
    {
        Task<List<CMCalibrationList>> GetCalibrationList(int facility_id, string facilitytime);
        Task<CMCalibrationDetails> GetCalibrationDetails(int id, string facilitytime);
        Task<CMDefaultResponse> RequestCalibration(CMRequestCalibration request, int userID);
        Task<CMDefaultResponse> ApproveRequestCalibration(CMApproval request, int userID);
        Task<CMDefaultResponse> RejectRequestCalibration(CMApproval request, int userID);
        Task<CMPreviousCalibration> GetPreviousCalibration(int asset_id, string facilitytime);
        Task<CMDefaultResponse> StartCalibration(int calibration_id, int userID, string facilitytime);
        Task<CMDefaultResponse> CompleteCalibration(CMCompleteCalibration request, int userID);
        Task<CMDefaultResponse> CloseCalibration(CMCloseCalibration request, int userID);
        Task<CMRescheduleApprovalResponse> ApproveCalibration(CMApproval request, int userID);
        Task<CMDefaultResponse> RejectCalibration(CMApproval request, int userID);
        Task<CMDefaultResponse> SkipCalibration(CMCloseCalibration request, int userID);
    }
    public class CalibrationBS : ICalibrationBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public CalibrationBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMCalibrationList>> GetCalibrationList(int facility_id, string facilitytime)
        {
            try
            {
                using (var repos = new CalibrationRepository(getDB))
                {
                    return await repos.GetCalibrationList(facility_id, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMCalibrationDetails> GetCalibrationDetails(int id, string facilitytime)
        {
            try
            {
                using (var repos = new CalibrationRepository(getDB))
                {
                    return await repos.GetCalibrationDetails(id, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RequestCalibration(CMRequestCalibration request, int userID)
        {
            try
            {
                using (var repos = new CalibrationRepository(getDB))
                {
                    return await repos.RequestCalibration(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveRequestCalibration(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new CalibrationRepository(getDB))
                {
                    return await repos.ApproveRequestCalibration(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectRequestCalibration(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new CalibrationRepository(getDB))
                {
                    return await repos.RejectRequestCalibration(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMPreviousCalibration> GetPreviousCalibration(int asset_id, string facilitytime)
        {
            try
            {
                using (var repos = new CalibrationRepository(getDB))
                {
                    return await repos.GetPreviousCalibration(asset_id, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> StartCalibration(int calibration_id, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new CalibrationRepository(getDB))
                {
                    return await repos.StartCalibration(calibration_id, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CompleteCalibration(CMCompleteCalibration request, int userID)
        {
            try
            {
                using (var repos = new CalibrationRepository(getDB))
                {
                    return await repos.CompleteCalibration(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CloseCalibration(CMCloseCalibration request, int userID)
        {
            try
            {
                using (var repos = new CalibrationRepository(getDB))
                {
                    return await repos.CloseCalibration(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMRescheduleApprovalResponse> ApproveCalibration(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new CalibrationRepository(getDB))
                {
                    return await repos.ApproveCalibration(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectCalibration(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new CalibrationRepository(getDB))
                {
                    return await repos.RejectCalibration(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> SkipCalibration(CMCloseCalibration request, int userID)
        {
            try
            {
                using (var repos = new CalibrationRepository(getDB))
                {
                    return await repos.SkipCalibration(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
