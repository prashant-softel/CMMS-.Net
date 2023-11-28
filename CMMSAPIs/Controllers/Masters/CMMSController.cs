using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.BS.Masters;
using CMMSAPIs.Helper;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;




namespace CMMSAPIs.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class CMMSController : ControllerBase
    {
        private readonly ICMMSBS _CMMSBS;
        public CMMSController(ICMMSBS cmms)
        {
            _CMMSBS = cmms;
        }     

        #region helper

        [Route("GetFacility")]
        [HttpGet]
        public async Task<IActionResult> GetFacility(int facility_id)
        {
            try
            {
                var data = await _CMMSBS.GetFacility(facility_id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetFacilityList")]
        [HttpGet]
        public async Task<IActionResult> GetFacilityList()
        {
            try
            {
                var data = await _CMMSBS.GetFacilityList();
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetBlockList")]
        [HttpGet]
        public async Task<IActionResult> GetBlockList(int facility_id)
        {
            try
            {
                var data = await _CMMSBS.GetBlockList(facility_id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetAssetCategoryList")]
        [HttpGet]
        public async Task<IActionResult> GetAssetCategoryList()
        {
            try
            {
                var data = await _CMMSBS.GetAssetCategoryList();
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetAssetsList")]
        [HttpGet]
        public async Task<IActionResult> GetAssetsList(int facility_id)
        {
            try
            {
                var data = await _CMMSBS.GetAssetList(facility_id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetEmployeeList")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeeList(int facility_id, CMMS.CMMS_Modules module, CMMS.CMMS_Access access)
        {
            try
            {
                var data = await _CMMSBS.GetEmployeeList(facility_id, module, access);
                return Ok(data);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetBusinessTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetBusinessTypeList()
        {
            try
            {
                var data = await _CMMSBS.GetBusinessTypeList();
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetBusinessList")]
        [HttpGet]
        public async Task<IActionResult> GetBusinessList(int businessType)
        {
            try
            {
                var data = await _CMMSBS.GetBusinessList(businessType);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("AddBusinessType")]
        [HttpPost]
        public async Task<IActionResult> AddBusinessType(CMBusinessType request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CMMSBS.AddBusinessType(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("UpdateBusinessType")]
        [HttpPatch]
        public async Task<IActionResult> UpdateBusinessType(CMBusinessType request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CMMSBS.UpdateBusinessType(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteBusinessType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteBusinessType(int id, int userId)
        {
            try
            {
                var data = await _CMMSBS.DeleteBusinessType(id, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("AddBusiness")]
        [HttpPost]
        public async Task<IActionResult> AddBusiness(List<CMBusiness> request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CMMSBS.AddBusiness(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("UpdateBusiness")]
        [HttpPatch]
        public async Task<IActionResult> UpdateBusiness(CMBusiness request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CMMSBS.UpdateBusiness(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteBusiness")]
        [HttpDelete]
        public async Task<IActionResult> DeleteBusiness(int id, int userId)
        {
            try
            {
                var data = await _CMMSBS.DeleteBusiness(id, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("ImportBusiness")]
        [HttpPost]
        public async Task<IActionResult> ImportBusiness(int file_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CMMSBS.ImportBusiness(file_id, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetFinancialYear")]
        [HttpGet]
        public async Task<IActionResult> GetFinancialYear()
        {
            try
            {
                var data = await _CMMSBS.GetFinancialYear();
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Route("GetBloodGroupList")]
        [HttpGet]
        public async Task<IActionResult> GetBloodGroupList()
        {
            try
            {
                var data = await _CMMSBS.GetBloodGroupList();
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Route("GetGenderList")]
        [HttpGet]
        public async Task<IActionResult> GetGenderList()
        {
            try
            {
                var data = await _CMMSBS.GetGenderList();
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Route("GetRiskTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetRiskTypeList()
        {
            try
            {
                var data = await _CMMSBS.GetRiskTypeList();
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [Route("GetInsuranceStatusList")]
        [HttpGet]
        public async Task<IActionResult> GetInsuranceStatusList()
        {
            try
            {
                var data = await _CMMSBS.GetInsuranceStatusList();
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Route("GetInsuranceProviderList")]
        [HttpGet]
        public async Task<IActionResult> GetInsuranceProviderList()
        {
            try
            {
                var data = await _CMMSBS.GetInsuranceProviderList();
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Route("GetSPVList")]
        [HttpGet]
        public async Task<IActionResult> GetSPVList()
        {
            try
            {
                var data = await _CMMSBS.GetSPVList();
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        //[Authorize]
        [Route("CreateRiskType")]
        [HttpPost]
        public async Task<IActionResult> CreateRiskType(CMIRRiskType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CMMSBS.CreateRiskType(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("UpdateRiskType")]
        [HttpPatch]
        public async Task<IActionResult> UpdateRiskType(CMIRRiskType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CMMSBS.UpdateRiskType(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("DeleteRiskType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRiskType(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CMMSBS.DeleteRiskType(id, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("CreateSPV")]
        [HttpPost]
        public async Task<IActionResult> CreateSPV(CMSPV request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CMMSBS.CreateSPV(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("UpdateSPV")]
        [HttpPatch]
        public async Task<IActionResult> UpdateSPV(CMSPV request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CMMSBS.UpdateSPV(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("DeleteSPV")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSPV(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CMMSBS.DeleteSPV(id, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Module CRUD
        [Route("AddModule")]
        [HttpPost]
        public async Task<IActionResult> AddModule(CMModule request)
        {
            try
            {
                var data = await _CMMSBS.AddModule(request);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("UpdateModule")]
        [HttpPatch]
        public async Task<IActionResult> UpdateModule(CMModule request)
        {
            try
            {
                var data = await _CMMSBS.UpdateModule(request);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("DeleteModule")]
        [HttpDelete]
        public async Task<IActionResult> DeleteModule(int id)
        {
            try
            {
                var data = await _CMMSBS.DeleteModule(id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetModuleList")]
        [HttpGet]
        public async Task<IActionResult> GetModuleList()
        {
            try
            {
                var data = await _CMMSBS.GetModuleList();
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetModuleDetail")]
        [HttpGet]
        public async Task<IActionResult> GetModuleDetail(int id)
        {
            try
            {
                var data = await _CMMSBS.GetModuleDetail(id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetStatusList")]
        [HttpGet]
        public async Task<IActionResult> GetStatusList(CMMS.CMMS_Modules module)
        {
            try
            {
                var data = await _CMMSBS.GetStatusList(module);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetFrequencyList")]
        [HttpGet]
        public async Task<IActionResult> GetFrequencyList()
        {
            try
            {
                var data = await _CMMSBS.GetFrequencyList();
                return Ok(data);
            }
            catch(Exception)
            {
                throw;
            }
        }

        [Route("Print")]
        [HttpGet]
        public async Task<FileResult> Print(int id, CMMS.CMMS_Modules moduleID)
        {
            try
            {

                //CMMSRepository obj = new CMMSRepository( );

                var data = await _CMMSBS.Print(id, moduleID);
                var body = data.ToString();

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    StringReader reader = new StringReader(body);
                    Document PdfFile = new Document(PageSize.A4);
                    PdfWriter writer = PdfWriter.GetInstance(PdfFile, stream);
                    PdfFile.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, reader);
                    PdfFile.Close();
                    return File(stream.ToArray(), "application/pdf", "Print.pdf");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DownloadFile")]
        [HttpGet]
        public async Task<FileResult> DownloadFile(int id, string filePath)
        {
            try
            {
                // Replace the following line with logic to get the template file path
                string path = "";
                if ((filePath == "" || filePath == null) && id > 0)
                {
                    path = await _CMMSBS.DownloadFile(id);
                }
                else if(filePath != null || filePath != "")
                {
                    path = filePath;
                }
                else
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    Response.WriteAsync("File not found.");
                }
     
                //string templateFilePath = $"./Upload/Templates/Bellary_Material.xlsx";
                string templateFilePath = path;
                byte[] fileContent = await System.IO.File.ReadAllBytesAsync(templateFilePath);
                string fileName = Path.GetFileName(templateFilePath);
                // Set the file response
                return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion //helper functions

        /*
        [Route("GetWindDailyGenSummary")]
        [HttpGet]

        
        [Route("eQry/{qry}")]
        [HttpGet]
        public async Task<IActionResult> eQry(string qry)
        {
            try
            {
                var data = await _CMMSBS.eQry(qry);
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        */

    }
}
