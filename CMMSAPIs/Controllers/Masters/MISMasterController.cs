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
using CMMSAPIs.BS.MISMasters;
using Newtonsoft.Json;
using CMMSAPIs.Models.Utils;
using System.Linq;

namespace CMMSAPIs.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class MISMasterController : ControllerBase
    {
        private readonly IMISMasterBS _IMISMasterBS;
        public MISMasterController(IMISMasterBS cmms)
        {
            _IMISMasterBS = cmms;
        }     

        #region helper

        [Route("GetSourceOfObservation")]
        [HttpGet]
        public async Task<IActionResult> GetSourceOfObservation(int source_id)
        {
            try
            {
                var data = await _IMISMasterBS.GetSourceOfObservation(source_id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetSourceOfObservationList")]
        [HttpGet]
        public async Task<IActionResult> GetSourceOfObservationList()
        {
            try
            {
                var data = await _IMISMasterBS.GetSourceOfObservationList();
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //[Authorize]
        [Route("AddSourceOfObservation")]
        [HttpPost]
        public async Task<IActionResult> AddSourceOfObservation(MISSourceOfObservation request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.AddSourceOfObservation(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("UpdateSourceOfObservation")]
        [HttpPatch]
        public async Task<IActionResult> UpdateSourceOfObservation(MISSourceOfObservation request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateSourceOfObservation(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteSourceOfObservation")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSourceOfObservation(int id, int userId)
        {
            try
            {
                var data = await _IMISMasterBS.DeleteSourceOfObservation(id, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* TYPE OF OBSERVATION *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/

        [Route("GetTypeOfObservation")]
        [HttpGet]
        public async Task<IActionResult> GetTypeOfObservation(int type_id)
        {
            try
            {
                var data = await _IMISMasterBS.GetTypeOfObservation(type_id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Route("GetTypeOfObservationList")]
        [HttpGet]
        public async Task<IActionResult> GetTypeOfObservationList()
        {
            try
            {
                var data = await _IMISMasterBS.GetTypeOfObservationList();
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //[Authorize]
        [Route("AddTypeOfObservation")]
        [HttpPost]
        public async Task<IActionResult> AddTypeOfObservation(MISTypeObservation request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.AddTypeOfObservation(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("UpdateTypeOfObservation")]
        [HttpPatch]
        public async Task<IActionResult> UpdateTypeOfObservation(MISTypeObservation request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateTypeOfObservation(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteTypeOfObservation")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTypeOfObservation(int id, int userId)
        {
            try
            {
                var data = await _IMISMasterBS.DeleteTypeOfObservation(id, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* GrievanceType *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/

        [Route("GetGrievanceType")]
        [HttpGet]
        public async Task<IActionResult> GetGrievanceType(int id)
        {
            try
            {
                var data = await _IMISMasterBS.GetGrievanceType(id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Route("GetGrievanceTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetGrievanceTypeList()
        {
            try
            {
                var data = await _IMISMasterBS.GetGrievanceTypeList();
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //[Authorize]
        [Route("AddGrievanceType")]
        [HttpPost]
        public async Task<IActionResult> AddGrievanceType(MISGrievanceType request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.AddGrievanceType(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("UpdateGrievanceType")]
        [HttpPatch]
        public async Task<IActionResult> UpdateGrievanceType(MISGrievanceType request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateGrievanceType(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteGrievanceType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteGrievanceType(int id, int userId)
        {
            try
            {
                var data = await _IMISMasterBS.DeleteGrievanceType(id, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* ResolutionLevel *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/

        [Route("GetResolutionLevel")]
        [HttpGet]
        public async Task<IActionResult> GetResolutionLevel(int id)
        {
            try
            {
                var data = await _IMISMasterBS.GetResolutionLevel(id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Route("GetResolutionLevelList")]
        [HttpGet]
        public async Task<IActionResult> GetResolutionLevelList()
        {
            try
            {
                var data = await _IMISMasterBS.GetResolutionLevelList();
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //[Authorize]
        [Route("AddResolutionLevel")]
        [HttpPost]
        public async Task<IActionResult> AddResolutionLevel(MISResolutionLevel request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.AddResolutionLevel(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("UpdateResolutionLevel")]
        [HttpPatch]
        public async Task<IActionResult> UpdateResolutionLevel(MISResolutionLevel request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateResolutionLevel(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteResolutionLevel")]
        [HttpDelete]
        public async Task<IActionResult> DeleteResolutionLevel(int id, int userId)
        {
            try
            {
                var data = await _IMISMasterBS.DeleteResolutionLevel(id, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* RISK TYPE *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        [Route("GetRiskType")]
        [HttpGet]
        public async Task<IActionResult> GetRiskType(int risk_id)
        {
            try
            {
                var data = await _IMISMasterBS.GetRiskType(risk_id);
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
                var data = await _IMISMasterBS.GetRiskTypeList();
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
        public async Task<IActionResult> CreateRiskType(MISRiskType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateRiskType(request, userID);
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
        public async Task<IActionResult> UpdateRiskType(MISRiskType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateRiskType(request, userID);
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
                var data = await _IMISMasterBS.DeleteRiskType(id, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* COST TYPE *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        [Route("GetCostType")]
        [HttpGet]
        public async Task<IActionResult> GetCostType(int cost_id)
        {
            try
            {
                var data = await _IMISMasterBS.GetCostType(cost_id);
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Route("GetCostTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetCostTypeList()
        {
            try
            {
                var data = await _IMISMasterBS.GetCostTypeList();
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //[Authorize]
        [Route("CreateCostType")]
        [HttpPost]
        public async Task<IActionResult> CreateCostType(MISCostType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateCostType(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("UpdateCostType")]
        [HttpPatch]
        public async Task<IActionResult> UpdateCostType(MISCostType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateCostType(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("DeleteCostType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCostType(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteCostType(id, userID);
                return Ok(data);
            }
            catch (Exception)
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
        [Route("GetBodyPartsList")]
        [HttpGet]
        public async Task<IActionResult> GetBodyPartsList()
        {
            try
            {
                var data = await _IMISMasterBS.GetBodyPartsList();
                return Ok(data);
            }
            catch
            {
                throw;
            }
        }
        [Route("CreateBodyParts")]
        [HttpPost]
        public async Task<IActionResult> CreateBodyParts(BODYPARTS request, int Userid)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateBodyParts(request, userID);
                return Ok(data);
            }
            catch
            {
                throw;
            }
        }
        [Route("UpdateBodyParts")]
        [HttpPut]
        public async Task<IActionResult> UpdateBodyParts(BODYPARTS request, int UserId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateBodyParts(request, userID);
                return Ok(data);
            }
            catch
            {
                throw;
            }
        }
        [Route("DeleteBodyParts")]
        [HttpDelete]
        public async Task<IActionResult> DeleteBodyParts(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteBodyParts(id, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
            [Route("GetResponsibilityList")]
            [HttpGet]
            public async Task<IActionResult> GetResponsibilityList()
            {
                try
                {
                    var data = await _IMISMasterBS.GetResponsibilityList();
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            [Route("GetResponsibilityID")]
            [HttpGet]
            public async Task<IActionResult> GetResponsibilityID(int id ,int facility_id)
            {
                try
                {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _IMISMasterBS.GetResponsibilityID(id,facilitytimeZone);
                    return Ok(data);

                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }

            }
            [Route("CreateResponsibility")]
            [HttpPost]
            public async Task<IActionResult> CreateResponsibility(Responsibility request, int UserID)
            {

                try
                {
                    int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                    var data = await _IMISMasterBS.CreateResponsibility(request, UserID);
                    return Ok(data);
                }
                catch (Exception)
                {
                    throw;
                }

            }
            [Route("UpdateResponsibility")]
            [HttpPut]
            public async Task<IActionResult> UpdateResponsibility(Responsibility request, int UserID)
            {

                try
                {
                    int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                    var data = await _IMISMasterBS.UpdateResponsibility(request, UserID);
                    return Ok(data);
                }
                catch (Exception)
                {
                    throw;
                }

            }
            [Route("DeleteResponsibility")]
            public async Task<IActionResult> DeleteResponsibility(int id)
            {
                try
                {
                    var data = await _IMISMasterBS.DeleteResponsibility(id);
                    return Ok(data);
                }
                catch
                {
                    throw;
                }
            }

        // Incident type CRUD APIS
        [Route("GetIncidentType")]
        [HttpGet]
        public async Task<IActionResult> GetIncidentType(int id)
        {
            try
            {
                var data = await _IMISMasterBS.GetIncidentType(id);
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Route("GetIncidentTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetIncidentTypeList(int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _IMISMasterBS.GetIncidentTypeList(facilitytimeZone);
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //[Authorize]
        [Route("CreateIncidentType")]
        [HttpPost]
        public async Task<IActionResult> CreateIncidentType(CMIncidentType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateIncidentType(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("UpdateIncidentType")]
        [HttpPost]
        public async Task<IActionResult> UpdateIncidentType(CMIncidentType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateIncidentType(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("DeleteIncidentType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteIncidentType(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteIncidentType(id, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // MIS Water Data APIS

        [Route("GetWaterDataById")]
        [HttpGet]
        public async Task<IActionResult> GetWaterDataById(int id,int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _IMISMasterBS.GetWaterDataById(id, facilitytimeZone);
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [Route("GetWaterDataList")]
        [HttpGet]

        public async Task<IActionResult> GetWaterDataList(DateTime fromDate, DateTime toDate,int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _IMISMasterBS.GetWaterDataList(fromDate, toDate, facilitytimeZone);
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [Route("GetWaterDataReport")]
        [HttpGet]
        public async Task<IActionResult> GetWaterDataReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _IMISMasterBS.GetWaterDataReport(fromDate, toDate);
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [Route("CreateWaterData")]
        [HttpPost]
        public async Task<IActionResult> CreateWaterData(CMMisWaterData request)
        {

            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateWaterData(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [Route("UpdateWaterData")]
        [HttpPut]
        public async Task<IActionResult> UpdateWaterData(CMMisWaterData request)
        {

            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateWaterData(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [Route("DeleteWaterData")]
        public async Task<IActionResult> DeleteWaterData(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteWaterData(id, userID);
                return Ok(data);
            }
            catch
            {
                throw;
            }
        }      
        [Route("GetWasteDataList")]
        [HttpGet]
        public async Task<IActionResult> GetWasteDataList(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _IMISMasterBS.GetWasteDataList(fromDate, toDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetWasteDataByID")]
        [HttpGet]
        public async Task<IActionResult> GetWasteDataByID(int Id)
        {
            try
            {
                var data = await _IMISMasterBS.GetWasteDataByID(Id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("CreateWasteData")]
        [HttpPost]
        public async Task<IActionResult> CreateWasteData(CMWasteData request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateWasteData(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("UpdateWasteData")]
        [HttpPost]
        public async Task<IActionResult> UpdateWasteData(CMWasteData request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateWasteData(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("DeleteWasteData")]
        [HttpPost]
        public async Task<IActionResult> DeleteWasteData(int Id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteWasteData(Id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //changes
        [Route("GetWaterType")]
        [HttpGet]
        public async Task<IActionResult> GetWaterType(int facility_Id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.GetWaterType(facility_Id);
                return Ok(data);
            }
            catch
            {
                throw;
            }
        }
        [Route("CreateWaterType")]
        [HttpPost]
        public async Task<IActionResult> CreateWaterType(WaterDataType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateWaterType(request,userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("UpdateWaterType")]
        [HttpPut]
        public async Task<IActionResult> UpdateWaterType(WaterDataType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateWaterType(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("DeleteWaterType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteWaterType(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteWaterType(id, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Route("GetWasteType")]
        [HttpGet]
        public async Task<IActionResult> GetWasteType(int facility_Id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.GetWasteType(facility_Id);
                return Ok(data);
            }
            catch
            {
                throw;
            }
        }

        [Route("GetWaterDataListMonthWise")]
        [HttpGet]
        public async Task<IActionResult> GetWaterDataListMonthWise(DateTime fromDate, DateTime toDate, int facility_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.GetWaterDataListMonthWise(fromDate, toDate, facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetWasteDataListMonthWise")]
        [HttpGet]
        public async Task<IActionResult> GetWasteDataListMonthWise(DateTime fromDate, DateTime toDate, int Hazardous, int facility_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.GetWasteDataListMonthWise(fromDate, toDate, Hazardous, facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetWaterDataMonthDetail")]
        [HttpGet]
        public async Task<IActionResult> GetWaterDataMonthDetail(int Month, int Year, int facility_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.GetWaterDataMonthDetail(Month, Year, facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetWasteDataMonthDetail")]
        [HttpGet]
        public async Task<IActionResult> GetWasteDataMonthDetail(int Month, int Year, int Hazardous, int facility_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.GetWasteDataMonthDetail(Month, Year, Hazardous, facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
