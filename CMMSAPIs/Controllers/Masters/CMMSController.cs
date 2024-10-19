using CMMSAPIs.BS.Masters;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        [Route("GetFacilityListByUserId")]
        [HttpGet]
        public async Task<IActionResult> GetFacilityListByUserId()
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CMMSBS.GetFacilityListByUserId(userID);
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
            catch (ArgumentException ex)
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
        public async Task<IActionResult> GetBusinessList(int businessType, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _CMMSBS.GetBusinessList(businessType, facilitytimeZone);
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
        public async Task<IActionResult> AddBusinessType(CMBusinessType request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
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
        public async Task<IActionResult> UpdateBusinessType(CMBusinessType request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
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
        public async Task<IActionResult> DeleteBusinessType(int id)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
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
        public async Task<IActionResult> AddBusiness(List<CMBusiness> request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
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
        public async Task<IActionResult> UpdateBusiness(CMBusiness request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
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
        public async Task<IActionResult> DeleteBusiness(int id)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
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
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CMMSBS.ImportBusiness(file_id, userId);
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
        public async Task<IActionResult> GetModuleList(bool return_all)
        {
            try
            {
                var data = await _CMMSBS.GetModuleList(return_all);
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
        public async Task<IActionResult> GetStatusList()
        {
            try
            {
                var data = await _CMMSBS.GetStatusList();
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
            catch (Exception)
            {
                throw;
            }
        }

        [Route("Print")]
        [HttpGet]
        public async Task<FileResult> Print(int id, CMMS.CMMS_Modules moduleID, int facilityId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;

                var data = await _CMMSBS.Print(id, moduleID, userID, facilitytimeZone);
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
                else if (filePath != null || filePath != "")
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

        [Route("getDashboadDetails")]
        [HttpGet]
        public async Task<IActionResult> getDashboadDetails(string facilityId, string moduleID, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _CMMSBS.getDashboadDetails(facilityId, moduleID, fromDate, toDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = StatusCodes.Status404NotFound;
                item.Message = ex.Message;
                return Ok(item);
            }
        }
        [Route("GetStatusbymodule")]
        [HttpGet]
        public async Task<IActionResult> GetStatusbymodule(CMMS.CMMS_Modules module)
        {
            try
            {
                var data = await _CMMSBS.GetStatusbymodule(module);
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [Route("GetEscalationModuleList")]
        [HttpGet]
        public async Task<IActionResult> GetEscalationModuleList()
        {
            try
            {
                var data = await _CMMSBS.GetEscalationModuleList();
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Route("GetEscalationStatusbymodule")]
        [HttpGet]
        public async Task<IActionResult> GetEscalationStatusbymodule(CMMS.CMMS_Modules module)
        {
            try
            {
                var data = await _CMMSBS.GetEscalationStatusbymodule(module);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
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
