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
                case CMMS.CMMS_Status.Grievance_UPDATED:
                    retValue = "Updated";
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
        /*  internal static int getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
          {
              int retValue;

              switch (m_notificationID)
              {
                  case CMMS.CMMS_Status.GRIEVANCE_ONGOING:
                      retValue = 1;
                      break;
                  case CMMS.CMMS_Status.GRIEVANCE_PENDING:
                      retValue = 2;
                      break;
                  case CMMS.CMMS_Status.GRIEVANCE_COMPLETED:
                      retValue = 3;
                      break;
                  default:
                      retValue = 0; // Use a default integer value for other cases
                      break;
              }
              return retValue;

          }*/

        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMGrievance InvObj)
        {
            string retValue = "Grievance";

            switch (notificationID)
            {
                case CMMS.CMMS_Status.Grievance_ADDED:
                    retValue += String.Format("Asset {0} Added by {1} at {2}</p>", InvObj.grievanceType, InvObj.createdByName, InvObj.createdAt);
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


        internal async Task<List<CMGrievance>> GetGrievanceList(string facilityId, string status, string startDate, string endDate, int selfView, int userId)
        {
            // validate facilityid is not empty
            if (facilityId?.Length <= 0)
            {
                throw new ArgumentException($"Invalid facility id {facilityId}");
            }

            string myQuery =
                "SELECT g.id, g.facilityId, g.grievanceType AS grievanceTypeId, t.name AS grievanceType, g.concern, g.actionTaken, g.resolutionLevel, g.closedDate, g.status_id as statusId " +
                ", g.createdAt, g.createdBy, g.updatedBy, t.description, t.status, t.addedBy, t.addedAt " +
                ", t.updatedBy, t.updatedAt " +
                " FROM mis_grievance g " +
                " JOIN mis_m_grievancetype t ON g.grievanceType = t.id" +
                " WHERE  g.facilityId IN (" + facilityId + ") and g.status = 1";

           /* if (startDate?.Length > 0 && endDate?.Length > 0)
            {
                DateTime start = DateTime.Parse(startDate);
                DateTime end = DateTime.Parse(endDate);
                if (DateTime.Compare(start, end) < 0)
                    myQuery += " AND DATE_FORMAT(g.createdAt,'%Y-%m-%d') BETWEEN \'" + startDate + "\' AND \'" + endDate + "\'";
            }*/

            if (selfView > 0)
                myQuery += " AND (g.createdBy = " + userId + " OR g.createdBy = " + userId + ")";


            if (status?.Length > 0)
            {
                myQuery += " AND g.status IN (" + status + ")";
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
            return Grievance;
        }
    

        internal async Task<CMGrievance> GetGrievanceDetails(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"Invalid id: {id}");
            }
            string myQuery =
                "SELECT g.id, g.facilityId, g.grievanceType AS grievance_Type_Id, g.concern, g.actionTaken, " +
                "g.resolutionLevel, g.closedDate, g.status_id as statusId, g.createdAt, g.createdBy as createdById, g.updatedBy as updatedById, " +
                "t.name AS grievance_Type, t.description, t.status, t.addedBy, t.addedAt, t.updatedBy, t.updatedAt " +
                "FROM mis_grievance g " +
                "JOIN mis_m_grievancetype t ON g.grievanceType = t.id " +
                "WHERE g.id = " + id;

            List<CMGrievance> _ViewInventoryList = await Context.GetData<CMGrievance>(myQuery).ConfigureAwait(false);

            return _ViewInventoryList[0];
        }


        internal async Task<CMDefaultResponse> CreateGrievance(CMCreateGrievance request, int userID)
        {
            /*
             * Add all data in assets table and warranty table
            */

            int count = 0;
            int retID = 0;
            string concern = "";
            CMMS.RETURNSTATUS retCode = CMMS.RETURNSTATUS.FAILURE;
            string strRetMessage = "";
            int statusId = (int)CMMS.CMMS_Status.GRIEVANCE_ONGOING;


            List<int> idList = new List<int>();

            //foreach (varrequest in request)
            {

                string qry = "INSERT INTO  mis_grievance (facilityId, grievanceType, concern, actionTaken, resolutionLevel, closedDate, status_id, createdBy ) VALUES ";
                count++;
                concern = request.concern;
                if (concern.Length <= 0)
                {
                    throw new ArgumentException($" name of grievance cannot be empty on line {count}");
                }

                
                qry += "('" + request.facilityId + "','" + request.grievanceType + "','" +request.concern + "','" +request.actionTaken + "','" +request.resolutionLevel + "','" +request.closedDate + "','" + statusId + "','" + request.createdBy + "'); ";
                qry += "select LAST_INSERT_ID(); ";

                //List<CMGrievanceList> newGrievance = await Context.GetData<CMGrievanceList>(qry).ConfigureAwait(false);
                DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
                retID = Convert.ToInt32(dt.Rows[0][0]);

                idList.Add(retID);
                CMGrievance _GrievanceAdded = await GetGrievanceDetails(retID);

                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.Grievance, CMMS.CMMS_Status.Grievance_ADDED);
                _GrievanceAdded.statusShort = _shortStatus;
               // String _shortStatus = getShortStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_ADDED);
               // _inventoryAdded.status_short = _shortStatus;

                //_GrievanceAdded.status_short = Convert.ToInt32(_shortStatus);

                string _longStatus = getLongStatus(CMMS.CMMS_Modules.Grievance, CMMS.CMMS_Status.Grievance_ADDED, _GrievanceAdded);
                _GrievanceAdded.statusLong = _longStatus;

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.Grievance, CMMS.CMMS_Status.Grievance_ADDED, new[] { userID }, _GrievanceAdded);

                /* int _shortStatus = getShortStatus(CMMS.CMMS_Modules.Grievance, CMMS.CMMS_Status.Grievance_ADDED);
                 _GrievanceAdded.status_short = _shortStatus;*/


                //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.Grievance, CMMS.CMMS_Status.Grievance_ADDED, new[] { userID }, _GrievanceAdded);
            }
            if (count > 0)
            {

                retCode = CMMS.RETURNSTATUS.SUCCESS;
                if (count == 1)
                {
                    strRetMessage = "New grievance <" + concern + "> created";
                }
                else
                {
                    strRetMessage = "<" + count + "> new concern added";
                }
            }
            else
            {
                strRetMessage = "No grievance to add";
            }


            return new CMDefaultResponse(idList, retCode, strRetMessage);
        }


        internal async Task<CMDefaultResponse> UpdateGrievance(CMUpdateGrievance request, int userID)
        {
            string updateQry = "UPDATE mis_grievance SET ";

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid ID for updating grievance.");
            }

            bool updated = false; // A flag to track if any updates were made


            if (!string.IsNullOrEmpty(request.grievance))
            {
                updateQry += $"grievance = '{request.grievance}', ";
                updated = true;
            }

            if (request.grievanceType > 0)
            {
                updateQry += $"grievanceType = {request.grievanceType}, ";
                updated = true;
            }


            if (!string.IsNullOrEmpty(request.concern))
            {
                updateQry += $"concern = '{request.concern}', ";
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
            if (request.updatedBy > 0)
            {
                updateQry += $"updatedBy = {userID}, ";
                updateQry += $"updatedAt = {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}', ";
                updated = true;
            }



            if (updated)
            {
                // Remove the trailing comma and add the WHERE clause
                updateQry = updateQry.TrimEnd(',', ' ') + $" WHERE id = '{request.id}'";

                CMDefaultResponse obj = new CMDefaultResponse(request.id, CMMS.RETURNSTATUS.SUCCESS, $"Grievance <{request.id}> has been updated");

                CMGrievance _GrievanceUpdated = await GetGrievanceDetails(request.id);

                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.Grievance,CMMS.CMMS_Status.Grievance_UPDATED);
                _GrievanceUpdated.statusShort = _shortStatus;

                string _longStatus = getLongStatus(CMMS.CMMS_Modules.Grievance, CMMS.CMMS_Status.Grievance_UPDATED, _GrievanceUpdated);
                _GrievanceUpdated.statusLong = _longStatus;

                // You can uncomment the notification part when ready to use it
                //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.Grievance, CMMS.CMMS_Status.Grievance_UPDATED, new[] { userID }, _GrievanceUpdated);

                return obj;
            }
            else
            {
                throw new ArgumentException("No valid fields to update in the grievance.");
            }
        }

        internal async Task<CMDefaultResponse> DeleteGrievance(int id, int userID)
        {
           /* ?ID=34
             delete from assets and warranty table*/
           
        //Your code goes here
        if (id <= 0)
        {
            throw new ArgumentException("Invalid argument <" + id + ">");

        }

        CMGrievance _GrievanceAdded = await GetGrievanceDetails(id);

        string qry = $"SELECT concern as deleted_by FROM mis_grievance WHERE id = {id}";

        List<CMGrievance> deleted_by = await Context.GetData<CMGrievance>(qry).ConfigureAwait(false);

        _GrievanceAdded.deletedBy = deleted_by[0].deletedBy;

        string _shortStatus = getShortStatus(CMMS.CMMS_Modules.Grievance, CMMS.CMMS_Status.Grievance_DELETED);
        _GrievanceAdded.statusShort = _shortStatus;

        string _longStatus = getLongStatus(CMMS.CMMS_Modules.Grievance, CMMS.CMMS_Status.Grievance_DELETED, _GrievanceAdded);
        _GrievanceAdded.statusLong = _longStatus;

            await CMMSNotification.sendNotification(
     CMMS.CMMS_Modules.Grievance,
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
            obj = new CMDefaultResponse(id, CMMS.RETURNSTATUS.SUCCESS, "Grievance <" + id + "> has been deleted");
        }
        return obj;
        // DELETE t1, t2 FROM t1 INNER JOIN t2 INNER JOIN t3
        //WHERE t1.id = t2.id AND t2.id = t3.id;
    }



    }
}

