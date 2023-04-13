using CMMSAPIs.Helper;
using CMMSAPIs.Models.FileUpload;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.FileUpload;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.FileUpload
{
    public interface IFileUploadBS
    {
        Task<CMDefaultResponse> UploadFile(CMFileUpload request, int userID);
    }
    public class FileUploadBS : IFileUploadBS
    {
        private readonly DatabaseProvider databaseProvider;

        public static IWebHostEnvironment _environment;
        
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public FileUploadBS(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            _environment = environment;
            databaseProvider = dbProvider;
        }

        public async Task<CMDefaultResponse> UploadFile(CMFileUpload request, int userID)
        {
            try
            {
                using (var repos = new FileUploadRepository(getDB, _environment))
                {
                    return await repos.UploadFile(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
