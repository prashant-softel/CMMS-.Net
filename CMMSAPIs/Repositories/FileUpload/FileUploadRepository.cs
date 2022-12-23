using CMMSAPIs.Helper;
using CMMSAPIs.Models.FileUpload;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.FileUpload
{
    public class FileUploadRepository : GenericRepository
    {
        public static IWebHostEnvironment _environment;
        public FileUploadRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment environment) : base(sqlDBHelper)
        {
            _environment = environment;
        }

        internal async Task<CMDefaultResponse> UploadFile(CMFileUpload request)
        {
            try
            {
                foreach (var file in request.files)
                {
                    if (file.Length > 0)
                    {
                        if (!Directory.Exists(_environment.WebRootPath + "\\Upload"))
                        {
                            Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                        }
                        using (FileStream filestream = File.Create(_environment.WebRootPath + "\\Upload\\" + file.FileName))
                        {
                            file.CopyTo(filestream);
                            filestream.Flush();
                        }
                    }
                }
               
                CMDefaultResponse response = new CMDefaultResponse(0, 201);
                return response;
            }
            catch (Exception ex) 
            {
                throw;
            }            
        }
    }
}
