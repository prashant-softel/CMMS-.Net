using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Grievance;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.Models.Jobs;
using iTextSharp.tool.xml.html;
using Org.BouncyCastle.Asn1.X500;
using System.Drawing;
using System.Text.RegularExpressions;
using MySqlX.XDevAPI.Relational;
using Microsoft.IdentityModel.Tokens;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Users;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
//using static System.Net.WebRequestMethods;
//using IronXL;

namespace CMMSAPIs.Repositories.Grievance
{
    public class GrievanceRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        ErrorLog m_errorLog;
        public GrievanceRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment iwebhostobj) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(iwebhostobj);
        }

        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.Grievance_ADDED:
                    retValue = "Added";
                    break;
                case CMMS.CMMS_Status.GRIEVANCE_ONGOING:
                    retValue = "Updated";
                    break;
                case CMMS.CMMS_Status.GRIEVANCE_CLOSED:
                    retValue = "Closed";
                    break;
                case CMMS.CMMS_Status.Grievance_DELETED:
                    retValue = "Deleted";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;

        }
      

        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMGrievance InvObj)
        {
            string retValue = "Grievance";

            switch (notificationID)
            {
                case CMMS.CMMS_Status.Grievance_ADDED:
                    retValue += String.Format("Asset {0} Added by {1} at {2}</p>", InvObj.grievanceType, InvObj.createdByName, InvObj.createdAt);
                    break;
                case CMMS.CMMS_Status.GRIEVANCE_CLOSED:
                    retValue += String.Format("Asset {0} Added by {1} at {2}</p>", InvObj.grievanceType, InvObj.closedByName, InvObj.closedAt);
                    break;
                case CMMS.CMMS_Status.Grievance_UPDATED:
                    retValue += String.Format("Asset {0} Updated by {1} at {2}</p>", InvObj.grievanceType, InvObj.updatedBy, InvObj.updatedAt);
                    break;
                case CMMS.CMMS_Status.Grievance_DELETED:
                    retValue += String.Format("Asset {0} Deleted by {1} at {2}</p>", InvObj.grievanceType, InvObj.deletedBy, InvObj.deletedAt);
                    break;
                default:
                    break;
            }
            return retValue;

        }


        internal async Task<List<CMGrievance>> GetGrievanceList(string facilityId, string status, string startDate, string endDate, int selfView, string facilitytimezone)
        {
            // validate facilityid is not empty
            if (facilityId?.Length <= 0)
            {
                throw new ArgumentException($"Invalid facility id {facilityId}");
            }

            string myQuery =
                "SELECT g.id, g.facilityId, g.grievanceType AS grievanceTypeId, t.name AS grievanceType, g.concern, g.actionTaken, g.resolutionLevel, g.closedDate as closedAt, g.status_id as statusId " +
                ", g.createdAt, g.createdBy, g.updatedBy, g.description, t.status, t.addedBy, t.addedAt " +
                ", t.updatedBy, t.updatedAt " +
                " FROM mis_grievance g " +
                " JOIN mis_m_grievancetype t ON g.grievanceType = t.id" +
                " WHERE  g.facilityId IN (" + facilityId + ") and g.status = 1";

          

            if (status?.Length > 0)
            {
                myQuery += " AND g.status_id IN (" + status + ")";
            }



            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                DateTime start = DateTime.Parse(startDate);
                DateTime end = DateTime.Parse(endDate);
                if (DateTime.Compare(start, end) < 0)
                {
                    myQuery += " AND DATE_FORMAT(g.createdAt, '%Y-%m-%d') BETWEEN \'" + startDate + "\' AND \'" + endDate + "\'";
                }
            }

            List<CMGrievance> Grievance = await Context.GetData<CMGrievance>(myQuery).ConfigureAwait(false);
            foreach(var detail in Grievance)
            {
                detail.updatedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimezone, detail.updatedAt);
                detail.createdAt = await _utilsRepo.ConvertToUTCDTC(facilitytimezone, detail.createdAt);
                detail.deletedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimezone, detail.deletedAt);
            //    detail.closedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimezone, detail.closedAt);
            }
            return Grievance;
        }
    

        internal async Task<CMGrievance> GetGrievanceDetails(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"Invalid id: {id}");
            }
            string myQuery =
                "SELECT g.id, g.facilityId, g.grievanceType AS grievanceTypeId, g.concern, g.actionTaken, " +
                "g.resolutionLevel, g.description, g.closedDate, g.status_id as statusId, g.createdAt, g.createdBy as createdById, g.updatedBy as updatedById, " +
                "t.name AS grievanceType, t.description AS type_description, t.status, t.addedBy, t.addedAt, t.updatedBy, t.updatedAt " +
                "FROM mis_grievance g " +
                "JOIN mis_m_grievancetype t ON g.grievanceType = t.id " +
                "WHERE g.id = " + id;

            List<CMGrievance> _ViewGrievanceList = await Context.GetData<CMGrievance>(myQuery).ConfigureAwait(false);
            //update status
            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.GRIEVANCE, (CMMS.CMMS_Status)_ViewGrievanceList[0].statusId);
            _ViewGrievanceList[0].statusShort = _shortStatus;

            string _longStatus = getLongStatus(CMMS.CMMS_Modules.GRIEVANCE, (CMMS.CMMS_Status) _ViewGrievanceList[0].statusId, _ViewGrievanceList[0]);
            _ViewGrievanceList[0].statusLong = _longStatus;

            return _ViewGrievanceList[0];
        }


        internal async Task<CMDefaultResponse> CreateGrievance(CMCreateGrievance request, int userID)
        {
            /*
             * Add all data in assets table and warranty table
            */

            //int count = 0;
            int retID = 0;
            //string concern = "";
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE; // RETURN is defined as RETRUN
            string strRetMessage = "";
            int statusId = (int)CMMS.CMMS_Status.Grievance_ADDED;
            //request.status = 1;
            List<int> idList = new List<int>();

            {
              

                string qry = "INSERT INTO  mis_grievance (facilityId, grievanceType, concern, description, status_id, createdAt, createdBy, status ) VALUES ";
                
                //concern = request.concern;

                qry += "('" + request.facilityId + "','" + request.grievanceType + "','" + request.concern + "','" + request.description + "','" + statusId + "','" + UtilsRepository.GetUTCTime() + "','" + userID + "','" + request.status  + "'); ";

                qry += "select LAST_INSERT_ID(); ";

                //List<CMGrievanceList> newGrievance = await Context.GetData<CMGrievanceList>(qry).ConfigureAwait(false);
                DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
                retID = Convert.ToInt32(dt.Rows[0][0]);


                //idList.Add(retID);
                CMGrievance _GrievanceAdded = await GetGrievanceDetails(retID);

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.GRIEVANCE, CMMS.CMMS_Status.Grievance_ADDED, new int[] { userID }, _GrievanceAdded);


            }
            if (retID > 0)
            {

                retCode = CMMS.RETRUNSTATUS.SUCCESS;
               // if (count == 1)
                {
                    strRetMessage = "New grievance <" + request.concern + "> created";
                }
        
            }
            else
            {
                strRetMessage = "No grievance to add";
            }


            return new CMDefaultResponse(retID, retCode, strRetMessage);
        }


        internal async Task<CMDefaultResponse> UpdateGrievance(CMUpdateGrievance request, int userID)
        {
            string updateQry = "UPDATE mis_grievance SET ";

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid ID for updating grievance.");
            }

            bool updated = false; // A flag to track if any updates were made


           

            if (request.grievanceType > 0)
            {
                updateQry += $"grievanceType = {request.grievanceType}, ";
                updated = true;
            }


            if (!string.IsNullOrEmpty(request.description))
            {
                updateQry += $"description = '{request.description}', ";
                updated = true;
            }

            if (!string.IsNullOrEmpty(request.actionTaken))
            {
                updateQry += $"actionTaken = '{request.actionTaken}', ";
                updated = true;
            }

            if (request.resolutionLevel > 0)
            {
                updateQry += $"resolutionLevel = {request.resolutionLevel}, ";
                updated = true;
            }
            // if (request.updatedBy > 0)   wrong
            if (!string.IsNullOrEmpty(request.concern))
            {
                updateQry += $"concern = '{request.concern}', ";
                updated = true;
            }
            if (request.facilityId > 0)
            {
                updateQry += $"facilityId = {request.facilityId}, ";
                updated = true;
            }




            if (updated)
            {
                updateQry += $"updatedBy = {userID}, ";
                updateQry += $"updatedAt = '{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}', ";
                // Remove the trailing comma and add the WHERE clause
                updateQry = updateQry.TrimEnd(',', ' ') + $" WHERE id = '{request.id}'";

                CMDefaultResponse obj = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Grievance <{request.id}> has been updated");

                await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                CMGrievance _GrievanceUpdated = await GetGrievanceDetails(request.id);

/*                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.GRIEVANCE, CMMS.CMMS_Status.Grievance_UPDATED);
                _GrievanceUpdated.statusShort = _shortStatus;

                string _longStatus = getLongStatus(CMMS.CMMS_Modules.GRIEVANCE, CMMS.CMMS_Status.Grievance_UPDATED, _GrievanceUpdated);
                _GrievanceUpdated.statusLong = _longStatus;
*/
                // You can uncomment the notification part when ready to use it
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.GRIEVANCE, CMMS.CMMS_Status.Grievance_UPDATED, new[] { userID }, _GrievanceUpdated);

                return obj;
            }
            else
            {
                throw new ArgumentException("No valid fields to update in the grievance.");
            }
        }

        internal async Task<CMDefaultResponse> CloseGrievance(CMGrievance request, int userID)
        {
            int count = 0;
            int retID = 0;
            string concern = "";
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE; // RETURN is defined as RETRUN
            string strRetMessage = "";
            int statusId = (int)CMMS.CMMS_Status.GRIEVANCE_CLOSED;


            List<int> idList = new List<int>();


            string myQuery = "UPDATE jobs SET ";
            myQuery += $"closedAt = '{UtilsRepository.GetUTCTime()}', closedBy = {userID}, status = {statusId}, WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);


                idList.Add(retID);
                CMGrievance _GrievanceAdded = await GetGrievanceDetails(retID);

                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.GRIEVANCE, CMMS.CMMS_Status.Grievance_ADDED);
                _GrievanceAdded.statusShort = _shortStatus;
                // String _shortStatus = getShortStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_ADDED);
                // _inventoryAdded.status_short = _shortStatus;

                //_GrievanceAdded.status_short = Convert.ToInt32(_shortStatus);

                string _longStatus = getLongStatus(CMMS.CMMS_Modules.GRIEVANCE, CMMS.CMMS_Status.GRIEVANCE_CLOSED, _GrievanceAdded);
                _GrievanceAdded.statusLong = _longStatus;
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.GRIEVANCE, CMMS.CMMS_Status.GRIEVANCE_CLOSED, new int[] { userID }, _GrievanceAdded);
                //add to history

 

            return new CMDefaultResponse(idList, retCode, strRetMessage);
        }



       /* internal async Task<CMDefaultResponse> DeleteGrievance(int id, int userID)
        {
           *//* ?ID=34
             delete from assets and warranty table*//*
           
        //Your code goes here
        if (id <= 0)
        {
            throw new ArgumentException("Invalid argument <" + id + ">");

        }

        CMGrievance _GrievanceAdded = await GetGrievanceDetails(id);

        string qry = $"SELECT concern as deleted_by FROM mis_grievance WHERE id = {id}";

        List<CMGrievance> deleted_by = await Context.GetData<CMGrievance>(qry).ConfigureAwait(false);

        _GrievanceAdded.deletedBy = deleted_by[0].deletedBy;

        string _shortStatus = getShortStatus(CMMS.CMMS_Modules.GRIEVANCE, CMMS.CMMS_Status.Grievance_DELETED);
        _GrievanceAdded.statusShort = _shortStatus;

        string _longStatus = getLongStatus(CMMS.CMMS_Modules.GRIEVANCE, CMMS.CMMS_Status.Grievance_DELETED, _GrievanceAdded);
        _GrievanceAdded.statusLong = _longStatus;

            await CMMSNotification.sendNotification(
     CMMS.CMMS_Modules.GRIEVANCE,
     CMMS.CMMS_Status.Grievance_DELETED,
     new int[] { userID }, // Convert the single userID to an array
     _GrievanceAdded
 );
            string delQuery1 = $"UPDATE mis_grievance SET status_id = 0, status = 0 WHERE id = {id}";
      //string delQuery2 = $"UPDATE assetwarranty SET status = 0 where asset_id = {id}";
        await Context.GetData<List<int>>(delQuery1).ConfigureAwait(false);
      //await Context.GetData<List<int>>(delQuery2).ConfigureAwait(false);

        CMDefaultResponse obj = null;
        //if (retVal1 && retVal2)
        {
            obj = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Grievance <" + id + "> has been deleted");
        }
        return obj;
        // DELETE t1, t2 FROM t1 INNER JOIN t2 INNER JOIN t3
        //WHERE t1.id = t2.id AND t2.id = t3.id;
    }*/

         internal async Task<CMDefaultResponse> DeleteGrievance(int id, int userID)
        {
            /*?ID=34
             * delete from assets and warranty table
            */
            /*Your code goes here*/
            if (id <= 0)
            {
                throw new ArgumentException("Invalid argument <" + id + ">");

            }

           CMGrievance _grievanceAdded = await GetGrievanceDetails(id);

            string qry = $"SELECT concern as deleted_by FROM mis_grievance WHERE id = {id}";

            List<CMGrievance> deleted_by = await Context.GetData<CMGrievance>(qry).ConfigureAwait(false);

            _grievanceAdded.deletedBy = deleted_by[0].deletedBy;

            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.GRIEVANCE, CMMS.CMMS_Status.Grievance_DELETED);
            _grievanceAdded.statusShort = _shortStatus;

            string _longStatus = getLongStatus(CMMS.CMMS_Modules.GRIEVANCE, CMMS.CMMS_Status.Grievance_DELETED, _grievanceAdded);
            _grievanceAdded.statusLong = _longStatus;

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.GRIEVANCE, CMMS.CMMS_Status.Grievance_DELETED, new[] { userID }, _grievanceAdded);

            // Assuming UtilsRepository.GetUTCTime() returns a string in the format 'YYYY-MM-DD HH:mm:ss'
            string delQuery1 = $"UPDATE mis_grievance SET status = 0, DeletedAt = '{UtilsRepository.GetUTCTime()}', DeletedBy = '{userID}' WHERE id = {id}";

            // string delQuery2 = $"UPDATE assetwarranty SET status = 0 where asset_id = {id}";
            await Context.GetData<List<int>>(delQuery1).ConfigureAwait(false);
           // await Context.GetData<List<int>>(delQuery2).ConfigureAwait(false);

            //send deleted notification here

            CMDefaultResponse obj = null;
            //if (retVal1 && retVal2)
            {
                obj = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Grievance <" + id + "> has been deleted");
            }
            return obj;
            // DELETE t1, t2 FROM t1 INNER JOIN t2 INNER JOIN t3
            //WHERE t1.id = t2.id AND t2.id = t3.id;
        }



    }
}

