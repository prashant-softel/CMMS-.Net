using CMMSAPIs.BS.MISMasters;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetResponsibilityID(int id, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _IMISMasterBS.GetResponsibilityID(id, facilitytimeZone);
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
        public async Task<IActionResult> GetWaterDataById(int id, int facility_id)
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

        public async Task<IActionResult> GetWaterDataList(DateTime fromDate, DateTime toDate, int facility_id)
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
        public async Task<IActionResult> GetWasteDataList(int facility_id, DateTime fromDate, DateTime toDate, int isHazardous)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _IMISMasterBS.GetWasteDataList(facility_id, fromDate, toDate, isHazardous, facilitytimeZone);
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
        public async Task<IActionResult> GetWaterType()
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.GetWaterType();
                return Ok(data);
            }
            catch
            {
                throw;
            }
        }
        [Route("GetWaterTypebyId")]
        [HttpGet]
        public async Task<IActionResult> GetWaterTypebyId(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.GetWaterTypebyId(id);
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
                var data = await _IMISMasterBS.CreateWaterType(request, userID);
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

        [Route("GetWasteTypeByid")]
        [HttpGet]
        public async Task<IActionResult> GetWasteTypeByid(int Type)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.GetWasteTypeByid(Type);
                return Ok(data);
            }
            catch
            {
                throw;
            }
        }
        [Route("GetWasteType")]
        [HttpGet]
        public async Task<IActionResult> GetWasteType()
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.GetWasteType();
                return Ok(data);
            }
            catch
            {
                throw;
            }
        }

        //changes for wastetype
        [Route("CreateWasteType")]
        [HttpPost]
        public async Task<IActionResult> CreateWasteType(WasteDataType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateWasteType(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("UpdateWasteType")]
        [HttpPut]
        public async Task<IActionResult> UpdateWasteType(WasteDataType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateWasteType(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("DeleteWasteType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteWasteType(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteWasteType(id, userID);
                return Ok(data);
            }
            catch (Exception)
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

        [Route("GetChecklistInspection")]
        [HttpGet]
        public async Task<IActionResult> GetChecklistInspectionReport(string facility_id, int module_type, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _IMISMasterBS.GetChecklistInspectionReport(facility_id, module_type, fromDate, toDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetObservationSheetReport")]
        [HttpGet]
        public async Task<IActionResult> GetObservationSheetReport(string facility_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                var data = await _IMISMasterBS.GetObservationSheetReport(facility_id, start_date, end_date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetObservationSummaryReport")]
        [HttpGet]
        public async Task<IActionResult> GetObservationSummaryReport(string facility_id, string fromDate, string toDate)
        {
            try
            {
                var data = await _IMISMasterBS.GetObservationSummaryReport(facility_id, fromDate, toDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CloseObservation")]
        [HttpPost]
        public async Task<IActionResult> CloseObservation(CMApproval request, int check_point_type_id)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CloseObservation(request, userId, check_point_type_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [Route("GetStatutoryComplianceMasterById")]
        [HttpGet]
        public async Task<IActionResult> GetStatutoryComplianceMasterById(int id)
        {
            try
            {
                var data = await _IMISMasterBS.GetStatutoryComplianceMasterById(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetStatutoryComplianceMasterList")]
        [HttpGet]
        public async Task<IActionResult> GetStatutoryComplianceMasterList()
        {
            try
            {
                var data = await _IMISMasterBS.GetStatutoryComplianceMasterList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CreateStatutoryComplianceMaster")]
        [HttpPost]
        public async Task<IActionResult> CreateStatutoryComplianceMaster(CMStatutoryCompliance request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateStatutoryComplianceMaster(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("UpdateStatutoryComplianceMaster")]
        [HttpPost]
        public async Task<IActionResult> UpdateStatutoryComplianceMaster(CMStatutoryCompliance request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateStatutoryComplianceMaster(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("DeleteStatutoryComplianceMaster")]
        [HttpPost]
        public async Task<IActionResult> DeleteStatutoryComplianceMaster(CMStatutoryCompliance request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteStatutoryComplianceMaster(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("RejectStatutory")]
        [HttpPost]
        public async Task<IActionResult> RejectStatutory(CMApprovals request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.RejectStatutory(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("ApproveStatutory")]
        [HttpPost]
        public async Task<IActionResult> ApproveStatutory(CMApprovals request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.ApproveStatutory(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Master of Status of Application
        [Route("CreateStatusofAppliaction")]
        [HttpPost]
        public async Task<IActionResult> CreateStatusofAppliaction(MISTypeObservation request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateStatusofAppliaction(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("UpdateStatsofAppliaction")]
        [HttpPost]
        public async Task<IActionResult> UpdateStatsofAppliaction(MISTypeObservation request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateStatsofAppliaction(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetStatsofAppliaction")]
        [HttpGet]
        public async Task<IActionResult> GetStatsofAppliaction()
        {
            try
            {
                var data = await _IMISMasterBS.GetStatsofAppliaction();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("DeleteStatsofAppliaction")]
        [HttpDelete]
        public async Task<IActionResult> DeleteStatsofAppliaction(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteStatsofAppliaction(id, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("CreateStatutory")]
        [HttpPost]
        public async Task<IActionResult> CreateStatutory(CMCreateStatutory request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateStatutory(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("UpdateStatutory")]
        [HttpPost]
        public async Task<IActionResult> UpdateStatutory(CMCreateStatutory request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateStatutory(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetStatutoryList")]
        [HttpGet]
        public async Task<IActionResult> GetStatutoryList(int facility_id, string start_date, string end_date)
        {
            try
            {
                var data = await _IMISMasterBS.GetStatutoryList(facility_id, start_date, end_date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetStatutoryHistoryById")]
        [HttpGet]
        public async Task<IActionResult> GetStatutoryHistoryById(int compliance_id)
        {
            try
            {
                var data = await _IMISMasterBS.GetStatutoryHistoryById(compliance_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetStatutoryById")]
        [HttpGet]
        public async Task<IActionResult> GetStatutoryById(int id)
        {
            try
            {
                var data = await _IMISMasterBS.GetStatutoryById(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // document Master
        [Route("GetDocument")]
        [HttpGet]
        public async Task<IActionResult> GetDocument()
        {
            try
            {
                var data = await _IMISMasterBS.GetDocument();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("CreateDocument")]
        [HttpPost]
        public async Task<IActionResult> CreateDocument(MISTypeObservation request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateDocument(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("UpdateDocument")]
        [HttpPost]
        public async Task<IActionResult> UpdateDocument(MISTypeObservation request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateDocument(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("DeleteDocument")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteDocument(id, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("uploadDocument")]
        [HttpPost]
        public async Task<IActionResult> uploadDocument(CMDocumentVersion request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.uploadDocument(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("getDocuementList")]
        [HttpGet]
        public async Task<IActionResult> getDocuementList(int facility_id, string fromDate, string toDate)
        {
            try
            {
                var data = await _IMISMasterBS.getDocuementList(facility_id, fromDate, toDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 200;
                item.Message = ex.Message;
                return Ok(item);
            }
        }
        [Route("getDocuementListById")]
        [HttpGet]
        public async Task<IActionResult> getDocuementListById(int id, string sub_doc_name, string fromDate, string toDate)
        {
            try
            {
                var data = await _IMISMasterBS.getDocuementListById(id, sub_doc_name, fromDate, toDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetObservationList")]
        [HttpGet]
        public async Task<IActionResult> GetObservationList(int facility_Id, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _IMISMasterBS.GetObservationList(facility_Id, fromDate, toDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetObservationDetails")]
        [HttpGet]
        public async Task<IActionResult> GetObservationDetails(int observation_id, int check_point_type_id)
        {
            try
            {
                var data = await _IMISMasterBS.GetObservationDetails(observation_id, check_point_type_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("CreateObservation")]
        [HttpPost]
        public async Task<IActionResult> CreateObservation(CMObservation request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateObservation(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("UpdateObservation")]
        [HttpPost]
        public async Task<IActionResult> UpdateObservation(CMObservation request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateObservation(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("DeleteObservation")]
        [HttpDelete]
        public async Task<IActionResult> DeleteObservation(int id, string comment)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteObservation(id, userID, comment);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /*[Route("GetChecklistInspection")]
        [HttpGet]
        public async Task<IActionResult> GetChecklistInspectionReport()
        {
            try
            {
                var data = await _IMISMasterBS.GetChecklistInspection();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/
        //Chags for MIs MIS_OccupationalHealthData
        [Route("CreateHealthData")]
        [HttpPost]
        public async Task<IActionResult> CreateHealthData(OccupationalHealthData request, int Userid)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateHealthData(request, userID);
                return Ok(data);
            }
            catch
            {
                throw;
            }
        }
        [Route("UpdateHealthData")]
        [HttpPost]
        public async Task<IActionResult> UpdateHealthData(OccupationalHealthData request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateHealthData(request, userID);
                return Ok(data);
            }
            catch
            {
                throw;
            }
        }
        [Route("DeleteHealthData")]
        [HttpDelete]
        public async Task<IActionResult> DeleteHealthData(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteHealthData(id, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Route("GetHealthData")]
        [HttpGet]
        public async Task<IActionResult> GetHealthData()
        {
            try
            {
                var data = await _IMISMasterBS.GetHealthData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("CreateVisitsAndNotices")]
        [HttpPost]
        public async Task<IActionResult> CreateVisitsAndNotices(VisitsAndNotices request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateVisitsAndNotices(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("UpdateVisitsAndNotices")]
        [HttpPost]
        public async Task<IActionResult> UpdateVisitsAndNotices(VisitsAndNotices request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateVisitsAndNotices(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("DeleteVisitsAndNotices")]
        [HttpDelete]
        public async Task<IActionResult> DeleteVisitsAndNotices([FromQuery] int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteVisitsAndNotices(id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetVisitsAndNotices")]
        [HttpGet]
        public async Task<IActionResult> GetVisitsAndNotices()
        {
            try
            {
                var data = await _IMISMasterBS.GetVisitsAndNotices();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //CreateFuleDaa
        [Route("CreateFuelConsumption")]
        [HttpPost]
        public async Task<IActionResult> CreateFuelConsumption(FuelData request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateFuelConsumption(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("UpdateFuelConsumption")]
        [HttpPost]
        public async Task<IActionResult> UpdateFuelConsumption(FuelData request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));

                var data = await _IMISMasterBS.UpdateFuelConsumption(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("DeleteFuelConsumption")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFuelConsumption(int id)
        {
            try
            {

                var data = await _IMISMasterBS.DeleteFuelConsumption(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetFuelConsumption")]
        [HttpGet]
        public async Task<IActionResult> GetFuelConsumption()
        {
            try
            {
                var data = await _IMISMasterBS.GetFuelConsumption();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // changes in PlantationData
        [Route("CreatePlantationData")]
        [HttpPost]
        public async Task<IActionResult> CreatePlantationData(PlantationData request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));

                var data = await _IMISMasterBS.CreatePlantationData(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("UpdatePlantationData")]
        [HttpPost]
        public async Task<IActionResult> UpdatePlantationData(PlantationData request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdatePlantationData(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("DeletePlantationData")]
        [HttpDelete]
        public async Task<IActionResult> DeletePlantationData(int id)
        {
            try
            {
                var data = await _IMISMasterBS.DeletePlantationData(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetPlantationData")]
        [HttpGet]
        public async Task<IActionResult> GetPlantationData(int id)
        {
            try
            {
                var data = await _IMISMasterBS.GetPlantationData(id);
                return Ok(data);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //kizens Data
        [Route("CreateKaizensData")]
        [HttpPost]
        public async Task<IActionResult> CreateKaizensData(KaizensData request, int userID)
        {
            try
            {
                var data = await _IMISMasterBS.CreateKaizensData(request, userID);
                return Ok(data);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("UpdateKaizensData")]
        [HttpPost]
        public async Task<IActionResult> UpdateKaizensData(KaizensData request, int userID)
        {
            try
            {
                var data = await _IMISMasterBS.UpdateKaizensData(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("DeleteKaizensData")]
        [HttpDelete]
        public async Task<IActionResult> DeleteKaizensData(int id)
        {
            try
            {
                var data = await _IMISMasterBS.DeleteKaizensData(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetKaizensData")]
        [HttpGet]
        public async Task<IActionResult> GetKaizensData()
        {
            try
            {
                var data = await _IMISMasterBS.GetKaizensData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("Cumulativereport")]
        [HttpGet]
        public async Task<IActionResult> Cumulativereport(string facility_id, int module_id, string start_date, string end_date)
        {
            try
            {
                var data = await _IMISMasterBS.Cumulativereport(facility_id, module_id, start_date, end_date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("AssingtoObservation")]
        [HttpPost]
        public async Task<IActionResult> AssingtoObservation(AssignToObservation request, int check_point_type_id)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.AssingtoObservation(request, check_point_type_id, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Evalution 
        [Route("CreateEvaluationPlan")]
        [HttpPost]
        public async Task<IActionResult> CreateEvaluation(CMEvaluationCreate request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateEvaluation(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }

        [Route("ApproveEvaluationPlan")]
        [HttpPost]
        public async Task<IActionResult> ApproveEvaluation(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.ApproveEvaluation(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("EvaluationPlanRejected")]
        [HttpPost]
        public async Task<IActionResult> EvaluationPlanRejected(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.EvaluationPlanRejected(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }
        [Route("EvaluationPlanUpdated")]
        [HttpPost]
        public async Task<IActionResult> EvaluationPlanUpdated(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.EvaluationPlanUpdated(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }

        [Route("EvaluationPlanDeleted")]
        [HttpDelete]
        public async Task<IActionResult> EvaluationPlanDeleted(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.EvaluationPlanDeleted(id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }

        [Route("EvaluationTaskSchedule")]
        [HttpPost]
        public async Task<IActionResult> EvaluationTaskSchedule(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.EvaluationTaskSchedule(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }

        [Route("EvaluationTaskAssign")]
        [HttpPost]
        public async Task<IActionResult> EvaluationTaskAssign(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.EvaluationTaskAssign(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }
        [Route("EvaluationTaskLinkPtw")]
        [HttpPost]
        public async Task<IActionResult> EvaluationTaskLinkPtw(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.EvaluationTaskLinkPtw(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }

        [Route("EvaluationTaskStart")]
        [HttpPost]
        public async Task<IActionResult> EvaluationTaskStart(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.EvaluationTaskStart(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }

        [Route("EvaluationTaskClose")]
        [HttpPost]
        public async Task<IActionResult> EvaluationTaskClosed(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.EvaluationTaskClosed(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }

        [Route("EvaluationTaskCloseReject")]
        [HttpPost]
        public async Task<IActionResult> EvaluationTaskClosedReject(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.EvaluationTaskClosedReject(request, userID);
                return Ok(data);
            }
            catch (Exception ex)

            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }

        [Route("EvaluationTaskCloseApproved")]
        [HttpPost]
        public async Task<IActionResult> EvaluationTaskClosedApproved(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.EvaluationTaskClosedApproved(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }

        [Route("EvaluationTaskCancel")]
        [HttpPost]
        public async Task<IActionResult> EvaluationTaskCancelled(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.EvaluationTaskCancelled(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }

        [Route("EvaluationCancelReject")]
        [HttpPost]
        public async Task<IActionResult> EvaluationCancelledReject(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.EvaluationTaskCancelledReject(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }

        [Route("EvaluationCancelApproved")]
        [HttpPost]
        public async Task<IActionResult> EvaluationCancelledApproved(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.EvaluationTaskCancelledApproved(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }
        [Route("DeleteEvaluationPlan")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEvaluationPlan(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteEvaluationPlan(id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }
        [Route("GetEvaluationPlan")]
        [HttpDelete]
        public async Task<IActionResult> GetEvaluationPlan(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.GetEvaluationPlan(id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }

        [Route("ApproveObservation")]
        [HttpPut]
        public async Task<IActionResult> ApproveObservation(CMApproval request, int facilityId, int check_point_type_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.ApproveObservation(request, userId, facilitytimeZone, check_point_type_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("RejectObservation")]
        [HttpPut]
        public async Task<IActionResult> RejectObservation(CMApproval request, int facilityId, int check_point_type_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.RejectObservation(request, userId, facilitytimeZone, check_point_type_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("GetMisSummary")]
        [HttpGet]
        public async Task<IActionResult> GetMisSummary(string year, int facility_id)
        {
            try
            {

                var data = await _IMISMasterBS.GetMisSummary(year, facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }
        [Route("GeEnvironmentalSummary")]
        [HttpGet]
        public async Task<IActionResult> GeEnvironmentalSummary(string year, int facility_id)
        {
            try
            {

                var data = await _IMISMasterBS.GeEnvironmentalSummary(year, facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }
    }
}

