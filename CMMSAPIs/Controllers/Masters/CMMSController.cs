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
        [Route("AddBusiness")]
        [HttpPost]
        public async Task<IActionResult> AddBusiness(List<CMBusiness> request)
        {
            try
            {
                var data = await _CMMSBS.AddBusiness(request);
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
