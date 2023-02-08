﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;


namespace CMMSAPIs.Repositories.Inventory
{
    public class InventoryRepository : GenericRepository
    {
        public InventoryRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal async Task<List<CMInventoryList>> GetInventoryList(int facilityId)
        {
            /*
             * get all details mentioned in model
             * Primary Table - Assets
             * Asset_category - asset category nam
             * AssetWarranty - warranty related information 
             * AssetType - type of inventory 
             * AssetSatus - status of inventory, 
             * Business - owner, operator, customer
            */
            /*Your code goes here*/
            string myQuery = 
                "SELECT a.id ,a.name, a.description, b2.name as supplier_name, b5.name as operator_name, ac.name as category_name," +

                " f.name AS block_name,a2.name as parent_name,  custbl.name as customer,owntbl.name as owner_name, s.name AS status, b5.name AS operator_name" +

                " from assets as a " +
                "join assetcategories as ac on ac.id= a.categoryId " +
                "" +
                "join business as custbl on custbl.id = a.customerId " +
                "" +
                "join business as owntbl" + " on owntbl.id = a.ownerId " +
                "" +
                "JOIN assets as a2 ON a.parentId = a2.id " +
                "" +
                "JOIN assetstatus as s on s.id = a.statusId " +
                "" +
                "JOIN business AS b2 ON a.ownerId = b2.id " +

                "JOIN business as b5 ON b5.id = a.operatorId " +
                "" +
                "JOIN facilities as f ON f.id = a.blockId";
            if (facilityId != 0)
            {
                myQuery += " WHERE a.facilityId= " + facilityId;



            }
            List<CMInventoryList> inventory = await Context.GetData<CMInventoryList>(myQuery).ConfigureAwait(false);
            return inventory;
        }

            internal async Task<List<CMViewInventory>> GetInventoryDetails(int id)
            {
                /*
                * get all details mentioned in model
                * Primary Table - Assets
                * Asset_category - asset category nam
                * AssetWarranty - warranty related information 
                * AssetType - type of inventory 
                * AssetSatus - status of inventory, 
                * Business - owner, operator, customer
               */
                /*Your code goes here*/

                string myQuery = "SELECT a.name, a.description, s.name AS status, f.name AS block_name, a2.name as parent_name, " +
                    "b3.name as manufacturer_name, a.currency FROM assets AS a JOIN assetstatus as s on s.id = a.statusId " +
                    "JOIN facilities as f ON f.id = a.blockId JOIN assets as a2 ON a.parentId = a2.id " +
                    "JOIN business AS b2 ON a.ownerId = b2.id JOIN business AS b3 ON a.manufacturerId = b3.id";
            if (id != 0)
            {
                myQuery += " WHERE a.id= " + id;



            }
            List<CMViewInventory> _ViewInventoryList = await Context.GetData<CMViewInventory>(myQuery).ConfigureAwait(false);
            
            return _ViewInventoryList;

            
        }

        internal async Task<CMDefaultResponse> AddInventory(List<CMAddInventory> request)
        {
            /*
             * Add all data in assets table and warranty table
            */

            int count = 0;
            int retID = 0;
            string assetName = "";
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            string strRetMessage = "";
            string qry = "insert into assets (name, description, parentId, acCapacity, moduleQuantity, dcCapacity, categoryId, typeId, statusId, facilityId, blockId, customerId, ownerId,operatorId, manufacturerId,supplierId,serialNumber,warrantyId,createdBy,status,photoId,cost,currency,stockCount,specialTool,specialToolEmpId,firstDueDate,calibration_last_date,frequency,calibrationFrequency,calibrationReminderDays,retirementStatus,multiplier) values ";
            foreach (var unit in request)
            {
                count++;
                assetName = unit.name;
                string firstCalibrationDate    = unit.calibrationFirstDueDate.ToString("yyyy-MM-dd");
                string lastCalibrationDate = unit.calibrationLastDate.ToString("yyyy-MM-dd");
                qry += "('" + unit.name + "','" + unit.description + "','" + unit.parentId + "','" + unit.acCapacity + "','" + unit.moduleQuantity + "','" + unit.dcCapacity + "','" + unit.categoryId + "','" + unit.typeId + "','" + unit.statusId + "','" + unit.facilityId + "','" + unit.blockId + "','" + unit.customerId + "','" + unit.ownerId + "','" + unit.operatorId + "','" + unit.manufacturerId + "','" + unit.supplierId + "','" + unit.serialNumber + "','" + unit.warrantyId + "','" + unit.createdBy + "','" + unit.statusId + "','" + unit.photoId + "','" + unit.cost + "','" + unit.currency + "','" + unit.stockCount + "','" + unit.specialToolId + "','" + unit.specialToolEmpId + "','" + firstCalibrationDate + "','" + lastCalibrationDate + "','" + unit.calibrationFrequencyType + "','" + unit.calibrationFrequencyType + "','" + unit.calibrationReminderDays + "','"+ unit.retirementStatus+ "','"+ unit.multiplier + "'),";
            }
            if (count > 0)
            {
                qry = qry.Substring(0, (qry.Length - 1)) + ";" + "select LAST_INSERT_ID(); ";

                //List<CMInventoryList> newInventory = await Context.GetData<CMInventoryList>(qry).ConfigureAwait(false);
                DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
                retID = Convert.ToInt32(dt.Rows[0][0]);
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
                if (count == 1)
                {
                    strRetMessage = "New asset <" + assetName + "> added";
                }
                else
                {
                    strRetMessage = "<" + count + "> new assets added";
                }
            }
            else
            {
                strRetMessage = "No assets to add";
            }
            return new CMDefaultResponse(retID, retCode, strRetMessage);
        }

        internal async Task<CMDefaultResponse> UpdateInventory(CMAddInventory request)
        {

            /*
             * update all data in assets table and warranty table
            */
            /*Your code goes here*/
            // "SELECT* FROM assets JOIN assetwarranty ON assets.warrantyId = assetwarranty.id";

            // string updateQry = $" name = '{request.name}  ', description = ' {request.description}  ', parentId = ' {request.parentId}  ', acCapacity = '  {request.acCapacity}  ', moduleQuantity = '  {request.moduleQuantity}' , categoryId = ' {request.categoryId} ', typeId = ' {request.typeId} ', statusId = ' {request.statusId}' , facilityId = ' {request.facilityId} ', blockId = ' {request.blockId} ' customer id = ' {request.customerId}', owner Id = ' {request.ownerId} ', manufacturer Id = ' {request.manufacturerId} ', supplier Id = ' {request.supplierId} ', serial Number = ' {request.serialNumber}' warranty Id = ' {request.warrantyId} ', Created At = ' {request.createdAt}', Created By = '{request.createdBy} ', Updated At = '{request.updatedAt}' , Updated By = ' {request.updatedBy} ', status = ' {request.status} ', photo Id = '  {request.photoId}' , cost = ' {request.cost} ' currency = ' {request.currency} ', stock Count = ' {request.stockCount}' , Special Tool = ' {request.specialTool} ' , specialToolEmpId = ' {request.specialToolEmpId} ', first Due Date = ' {request.firstDueDate} ', frequency ' {request.frequency}' , Description Maintainence = ' {request.descriptionMaintainence} 'Calibration Frequency = ' {request.calibrationFrequency} ', Calibration Reminder = '{request.calibrationReminder}', retirement Status = ' {request.retirementStatus} ', Multiplier = '{request.multiplier}' ";
            /* if (request.id > 0)
             {
                 updateQry += $" WHERE a.id= '{request.id}'";

             }
            */

            string updateQry = "";
            if (request.name != null)
            {
                updateQry += $"name= '{request.name}',";
            }
            if (request.description != null)
            {
                updateQry += $" description = '{request.description}',";
            }
            if (request.parentId != 0)
            {
                updateQry += $" parentId= '{request.parentId}',";
            }
            if (request.acCapacity != 0)
            {
                updateQry += $" acCapacity= '{request.acCapacity}',";
            }
            if (request.categoryId != 0)
            {
                updateQry += $" categoryId= '{request.categoryId}',";
            }
            if (request.moduleQuantity != 0)
            {
                updateQry += $" moduleQuantity= '{request.moduleQuantity}',";

            }
            if (request.typeId != 0)
            {
                updateQry += $" typeId= '{request.typeId}',";

            }
            if (request.statusId != 0)
            {
                updateQry += $" status= '{request.statusId}',";
            }
            if (request.facilityId != 0)
            {
                updateQry += $" facilityId= '{request.facilityId}',";

            }
            if (request.blockId!= 0) //here you may have check if its not 0. verify during testig
            {
                updateQry += $" blockId= '{request.blockId}',";

            }
            if (request.customerId != 0)
            {
                updateQry += $" customerId= '{request.customerId}',";

            }
            if (request.ownerId != 0)
            {
                updateQry += $" ownerId= '{request.ownerId}',";

            }
            if (request.manufacturerId != 0)
            {
                updateQry += $" manufacturerid= '{request.manufacturerId}',";

            }
            if (request.supplierId != 0)
            {
                updateQry += $" supplierId= '{request.supplierId}',";

            }
            if (request.serialNumber != null)
            {
                updateQry += $" serialNumber= '{request.serialNumber}',";

            }
            if (request.warrantyId != 0)
            {
                updateQry += $" warrantyId= '{request.warrantyId}',";

            }
            
            if (request.updatedAt != 0)
            {
                updateQry += $" updatedAt= '{UtilsRepository.GetUTCTime()}',";

            }
            if (request.updatedBy != 0)
            {
                updateQry += $" updatedBy= '{request.updatedBy}',";

            }
            if (request.photoId != 0)
            {
                updateQry += $" photoId= '{request.photoId}',";

            }
            if (request.cost != 0)
            {
                updateQry += $" cost= '{request.cost}',";

            }
            if (request.stockCount != 0)
            {
                updateQry += $" stockCount= '{request.stockCount}',";

            }
            if (request.specialToolId != 0)
            {
                updateQry += $" specialTool= '{request.specialToolId}',";

            }
            if (request.specialToolEmpId != 0)
            {
                updateQry += $" specialToolEmpId= '{request.specialToolEmpId}',";

            }
            if (request.calibrationFirstDueDate != null)
            {
                updateQry += $" firstDueDate= '{request.calibrationFirstDueDate}',";

            }
            if (request.calibrationFrequencyType != 0)
            {
                updateQry += $" frequency = '{request.calibrationFrequencyType}',";

            }
            if (request.calibrationFrequencyType != 0)
            {
                updateQry += $" calibrationFrequency= '{request.calibrationFrequencyType}',";

            }
            if (request.calibrationReminderDays != 0)
            {
                updateQry += $" calibrationReminderDays = '{request.calibrationReminderDays}',";

            }
            if (request.retirementStatus != 0)
            {
                updateQry += $" retirementStatus= '{request.retirementStatus}',";

            }
            if (request.multiplier != 0)
            {
                updateQry += $" multiplier = '{request.multiplier}',";

            }
            if(updateQry != null)
            {
                updateQry = "UPDATE assets SET " + updateQry.Substring(0, updateQry.Length - 1);
                updateQry += $" WHERE id= '{request.id}'";
                await Context.GetData<List<int>>(updateQry).ConfigureAwait(false);                
            }
            CMDefaultResponse obj = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Inventory  <" + request.id + "> has been updated");
            return obj;





        }

        internal async Task<CMDefaultResponse> DeleteInventory(int id)
        {
            /*?ID=34
             * delete from assets and warranty table
            */
            /*Your code goes here*/
            if (id <= 0)
            {
                throw new ArgumentException("Invalid argument <" + id + ">");

            }
            string delQuery1 = $"DELETE FROM assets WHERE id = {id}";
            string delQuery2 = $"DELETE FROM assetwarranty where asset_id = {id}";
            await Context.GetData<List<int>>(delQuery1).ConfigureAwait(false);
            await Context.GetData<List<int>>(delQuery2).ConfigureAwait(false);

            CMDefaultResponse obj = null;
            //if (retVal1 && retVal2)
            {
                obj = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Inventory <" + id + "> has been deleted");
            }
            return obj;
            // DELETE t1, t2 FROM t1 INNER JOIN t2 INNER JOIN t3
            //WHERE t1.id = t2.id AND t2.id = t3.id;
        }

        internal async Task<List<CMInventoryTypeList>> GetInventoryTypeList()
        {
            /*
             * Fetch data from assetType
            */
            /*Your code goes here*/
            // "SELECT * FROM assetTypes";
            string myQuery = "SELECT id, name, description, status FROM assettypes where status = 1";
            List<CMInventoryTypeList> _InventoryTypeList = await Context.GetData<CMInventoryTypeList>(myQuery).ConfigureAwait(false);
            return _InventoryTypeList;
        }

        internal async Task<List<CMInventoryStatusList>> GetInventoryStatusList()
        {
            /*
             * Fetch data from assetStatus
            */
            /*Your code goes here*/
            // "SELECT * FROM assetStatus";
            string myQuery = "SELECT id, name, description, status FROM assetstatus where status = 1";
            List<CMInventoryStatusList> _InventoryStatusList = await Context.GetData<CMInventoryStatusList>(myQuery).ConfigureAwait(false);
            return _InventoryStatusList;

        }
        internal async Task<List<CMInventoryCategoryList>> GetInventoryCategoryList()
        {
            string myQuery = "SELECT id, name FROM assetcategories where status = 1";
            List<CMInventoryCategoryList> _AssetCategory = await Context.GetData<CMInventoryCategoryList>(myQuery).ConfigureAwait(false);
            return _AssetCategory;
        }

    }
}
