using CMMSAPIs.Helper;
using CMMSAPIs.Models.FileUpload;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;

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
        internal async Task<CMDefaultResponse> UploadFile(CMFileUpload request)
        {
            try
            {
                string UploadPath = _environment.WebRootPath + "\\Upload\\" + request.facility_id + "\\" + request.module_id + "\\" + request.id+"\\";
                if (!Directory.Exists(UploadPath))
                {
                    Directory.CreateDirectory(UploadPath);
                }
                
                foreach (var file in request.files)
                {
                    if (file.Length > 0)
                    {
                        using (FileStream filestream = File.Create(UploadPath + file.FileName))
                        {
                            file.CopyTo(filestream);
                            filestream.Flush();
                            //TODO Implement CreateThumbnail Function
                            //CreateThumbnail(filePath);
                        }
                    }
                }
               
                CMDefaultResponse response = new CMDefaultResponse(0, 201, "");
                return response;
            }
            catch (Exception ex) 
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
