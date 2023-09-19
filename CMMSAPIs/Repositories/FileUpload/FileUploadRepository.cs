using CMMSAPIs.Helper;
using CMMSAPIs.Models.FileUpload;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Data;
using System.Collections.Generic;

namespace CMMSAPIs.Repositories.FileUpload
{
    public class FileUploadRepository : GenericRepository
    {
        public static IWebHostEnvironment _environment;
        public FileUploadRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment environment) : base(sqlDBHelper)
        {
            _environment = environment;
        }

        /*
         * File Path will be something as below
         * FacilityID/Moduel/ID/All Files
        */
        internal async Task<CMDefaultResponse> UploadFile(CMFileUpload request, int userID)
        {
            try
            {
                //string UploadPath = $"{ _environment.WebRootPath }\\Upload\\{ request.facility_id }\\{ (int)request.module_type }\\{ request.module_ref_id }\\";
                // This is for Current Root path
                //string UploadPath = $"{ _environment.ContentRootPath}\\Upload\\{ request.facility_id }\\{ (int)request.module_type }\\{ request.module_ref_id }\\";
                string UploadPath = $"Upload\\{ request.facility_id }\\{ (int)request.module_type }\\{ request.module_ref_id }\\";
                if (!Directory.Exists(UploadPath))
                {
                    Directory.CreateDirectory(UploadPath);
                }
                List<int> idList = new List<int>(); 
                foreach (var file in request.files)
                {
                    if (file.Length > 0)
                    {
                        using (FileStream filestream = File.Create(UploadPath + file.FileName))
                        {
                            file.CopyTo(filestream);
                            filestream.Flush();
                            string path = filestream.Name.Replace(@"\", @"\\");
                            string relativeFilePath = Path.Combine(UploadPath, file.FileName);
                            //TODO Implement CreateThumbnail Function
                            //CreateThumbnail(filePath);
                            string myQuery = "INSERT INTO uploadedfiles(facility_id, module_type, module_ref_id, file_category, file_path, file_type, created_by, created_at, file_size, file_size_units, file_size_bytes)" + 
                                $"VALUES ({request.facility_id}, {(int)request.module_type}, {request.module_ref_id}, {request.file_category}, '{relativeFilePath.Replace(@"\", @"\\")}', '{file.ContentType}', {userID}, " +
                                $"'{Utils.UtilsRepository.GetUTCTime()}', {file.Length}, 'B', {file.Length}); " +
                                "SELECT LAST_INSERT_ID();";
                            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
                            int id = Convert.ToInt32(dt.Rows[0][0]);
                            idList.Add(id);

                        }
                    }
                }
                CMDefaultResponse response = new CMDefaultResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, $"{idList.Count} file(s) uploaded successfully");
                return response;
            }
            catch (Exception) 
            {
                throw;
            }            
        }

        // TODO Below function is not working.
        // Its thrwoing error on Image image = Image.FromFile(imageFile); that System.Drawing.Common is not supported for this platform
        //public static void CreateThumbnail(string imageFile)
        //{
        //    string dir = new FileInfo(imageFile).DirectoryName;
        //    string thmFilePath = Path.Combine(dir, "thumbnail.jpeg");

        //    Image image = Image.FromFile(imageFile);
        //    var thumbImage = image.GetThumbnailImage(64, 64, new Image.GetThumbnailImageAbort(() => false), IntPtr.Zero);
        //    thumbImage.Save(thmFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        //}
    }
}
