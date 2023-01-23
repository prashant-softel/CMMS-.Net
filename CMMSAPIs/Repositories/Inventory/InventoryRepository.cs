using CMMSAPIs.Controllers.utils;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Formats.Asn1;
using System.Threading.Tasks;


namespace CMMSAPIs.Repositories.Inventory
{
    public class InventoryRepository : GenericRepository
    {
        public InventoryRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal async Task<List<CMInventoryList>> GetInventoryList(int facility_id)
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
            if (facility_id != 0)
            {
                myQuery += " WHERE a.facilityId= " + facility_id;



            }
            List<CMInventoryList> inventory = await Context.GetData<CMInventoryList>(myQuery).ConfigureAwait(false);
            return inventory;
        }

            internal async Task<List<CMViewInventory>> ViewInventory(int id)
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
            /*Your code goes here*/
            //
            string qry = "insert into assets (id, name, description, parent_id, acCapacity, moduleQuantity, dcCapacity, category_Id, type_Id, status_Id, facility_Id, block_Id, customer_Id, owner_Id,operator_Id, manufacturer_Id,supplier_Id,serialNumber,warranty_Id,createdAt,createdBy,updatedAt,updatedBy,status,photoId,cost,currency,stockCount,specialTool,specialToolEmpId,firstDueDate,frequency,descriptionMaintainence,calibrationFrequency,calibrationReminder,retirementStatus,multiplier) values ";

            foreach (var unit in request)
            {
                qry += "('" + unit.id + "','" + unit.name + "','" + unit.description + "','" + unit.parent_id + "','" + unit.acCapacity + "','" + unit.moduleQuantity + "','" + unit.dcCapacity + "','" + unit.category_Id + "','" + unit.type_Id + "','" + unit.status_Id + "','" + unit.facility_Id + "','" + unit.block_Id + "','" + unit.customer_Id + "','" + unit.owner_Id + "','" + unit.operator_Id + "','" + unit.manufacturer_Id + "','" + unit.supplier_Id + unit.serialNumber + "','" + unit.warranty_Id + "','" + unit.createdAt + "','" + unit.createdBy + "','" + unit.updatedAt + "','" + unit.updatedBy + "','" + unit.status + unit.photoId + "','" + unit.cost + "','" + unit.currency + "','" + unit.stockCount + "','" + unit.specialTool + "','" + unit.specialToolEmpId + "','" + unit.firstDueDate + "','" + unit.frequency + "','" + unit.descriptionMaintainence + "','" + unit.calibrationFrequency + "','" + unit.calibrationReminder + "','"+ unit.retirementStatus+ "','"+ unit.multiplier + "'),";
            }
            int retID = await Context.ExecuteNonQry<int>(qry.Substring(0, (qry.Length - 1)) + ";").ConfigureAwait(false);

            return new CMDefaultResponse(retID, 1, "");

            }

        internal async Task<CMDefaultResponse> UpdateInventory(CMAddInventory request)
        {

            /*
             * update all data in assets table and warranty table
            */
            /*Your code goes here*/
            // "SELECT* FROM assets JOIN assetwarranty ON assets.warrantyId = assetwarranty.id";

            string updateQry = "UPDATE assets SET ";
               // string updateQry = $" name = '{request.name}  ', description = ' {request.description}  ', parent_id = ' {request.parent_id}  ', acCapacity = '  {request.acCapacity}  ', moduleQuantity = '  {request.moduleQuantity}' , categoryId = ' {request.category_Id} ', typeId = ' {request.type_Id} ', status_Id = ' {request.status_Id}' , facilityId = ' {request.facility_Id} ', blockId = ' {request.block_Id} ' customer id = ' {request.customer_Id}', owner Id = ' {request.owner_Id} ', manufacturer Id = ' {request.manufacturer_Id} ', supplier Id = ' {request.supplier_Id} ', serial Number = ' {request.serialNumber}' warranty Id = ' {request.warranty_Id} ', Created At = ' {request.createdAt}', Created By = '{request.createdBy} ', Updated At = '{request.updatedAt}' , Updated By = ' {request.updatedBy} ', status = ' {request.status} ', photo Id = '  {request.photoId}' , cost = ' {request.cost} ' currency = ' {request.currency} ', stock Count = ' {request.stockCount}' , Special Tool = ' {request.specialTool} ' , specialToolEmpId = ' {request.specialToolEmpId} ', first Due Date = ' {request.firstDueDate} ', frequency ' {request.frequency}' , Description Maintainence = ' {request.descriptionMaintainence} 'Calibration Frequency = ' {request.calibrationFrequency} ', Calibration Reminder = '{request.calibrationReminder}', retirement Status = ' {request.retirementStatus} ', Multiplier = '{request.multiplier}' ";
               /* if (request.id > 0)
                {
                    updateQry += $" WHERE a.id= '{request.id}'";

                }
               */

                if(request.name != null)
            {
                updateQry += $"name= '{request.name}',";
            }
            if (request.description != null)
            {
                updateQry += $" description = '{request.description}',";
            }
            if (request.parent_id != null)
            {
                updateQry += $" parentId= '{request.parent_id}',";
            }
            if (request.acCapacity != null)
            {
                updateQry += $" acCapacity= '{request.acCapacity}',";
            }
            if (request.category_Id != null)
            {
                updateQry += $" categoryId= '{request.category_Id}',";
            }
            if (request.moduleQuantity != null)
            {
                updateQry += $" moduleQuantity= '{request.moduleQuantity}',";

            }
            if (request.type_Id != null)
            {
                updateQry += $" typeId= '{request.type_Id}',";

            }
            if (request.status_Id != null)
            {
                updateQry += $" statusId= '{request.status_Id}',";

            }
            if (request.facility_Id != null)
            {
                updateQry += $" facilityId= '{request.facility_Id}',";

            }
            if (request.block_Id!= null)
            {
                updateQry += $" blockId= '{request.block_Id}',";

            }
            if (request.customer_Id != null)
            {
                updateQry += $" customerId= '{request.customer_Id}',";

            }
            if (request.owner_Id != null)
            {
                updateQry += $" ownerId= '{request.owner_Id}',";

            }
            if (request.manufacturer_Id != null)
            {
                updateQry += $" manufacturerid= '{request.manufacturer_Id}',";

            }
            if (request.supplier_Id != null)
            {
                updateQry += $" supplierId= '{request.supplier_Id}',";

            }
            if (request.serialNumber != null)
            {
                updateQry += $" serialNumber= '{request.serialNumber}',";

            }
            if (request.warranty_Id != null)
            {
                updateQry += $" warrantyId= '{request.warranty_Id}',";

            }
            
            if (request.updatedAt != null)
            {
                updateQry += $" updatedAt= '{UtilsRepository.GetUTCTime()}',";

            }
            if (request.updatedBy != null)
            {
                updateQry += $" updatedBy= '{request.updatedBy}',";

            }
            if (request.status != null)
            {
                updateQry += $" status= '{request.status}',";

            }
            if (request.photoId != null)
            {
                updateQry += $" photoId= '{request.photoId}',";

            }
            if (request.cost != null)
            {
                updateQry += $" cost= '{request.cost}',";

            }
            if (request.stockCount != null)
            {
                updateQry += $" stockCount= '{request.stockCount}',";

            }
            if (request.specialTool != null)
            {
                updateQry += $" specialTool= '{request.specialTool}',";

            }
            if (request.specialToolEmpId != null)
            {
                updateQry += $" specialToolEmpId= '{request.specialToolEmpId}',";

            }
            if (request.firstDueDate != null)
            {
                updateQry += $" firstDueDate= '{UtilsRepository.GetUTCTime()}',";

            }
            if (request.frequency != null)
            {
                updateQry += $" frequency = '{request.frequency}',";

            }
   
            if (request.descriptionMaintainence != null)
            {
                updateQry += $" descriptionMaintenance= '{request.descriptionMaintainence}',";

            }
            if (request.calibrationFrequency != null)
            {
                updateQry += $" calibrationFrequency= '{request.calibrationFrequency}',";

            }
            if (request.calibrationReminder!= null)
            {
                updateQry += $" calibrationReminderDays = '{request.calibrationReminder}',";

            }
            if (request.retirementStatus != null)
            {
                updateQry += $" retirementStatus= '{request.retirementStatus}',";

            }
            if (request.multiplier != null)
            {
                updateQry += $" multiplier = '{request.multiplier}',";

            }
            if(updateQry != null)
            {
                
                updateQry = updateQry.Substring(0, updateQry.Length - 1);
                updateQry += $" WHERE id= '{request.id}'";

                await Context.GetData<List<int>>(updateQry).ConfigureAwait(false);

                
                
            }
            CMDefaultResponse obj = new CMDefaultResponse(request.id, 200, "Inventory has been updated");
            return obj;





        }

            internal async Task<CMDefaultResponse> DeleteInventory(int id)
            {
            /*?ID=34
             * delete from assets and warranty table
            */
            /*Your code goes here*/
            if (id > 0) 
            {
                string delQuery = $"DELETE FROM assets WHERE id = {id}";
                string del2Query = $"DELETE FROM assetwarranty where asset_id = {id}";
                await Context.GetData<List<int>>(delQuery).ConfigureAwait(false);
                await Context.GetData<List<int>>(del2Query).ConfigureAwait(false);
            }


            CMDefaultResponse obj = new CMDefaultResponse(id, 200, "Inventory has been updated");
            return obj;



            // DELETE t1, t2 FROM t1 INNER JOIN t2 INNER JOIN t3
            //WHERE t1.id = t2.id AND t2.id = t3.id;




        }

            internal async Task<List<KeyValuePairs>> GetInventoryTypeList()
            {
            /*
             * Fetch data from assetType
            */
            /*Your code goes here*/
            // "SELECT * FROM assetTypes";
            string myQuery = "SELECT * FROM assettypes WHERE type = 1";
            List<KeyValuePairs> _InventoryTypeList = await Context.GetData<KeyValuePairs>(myQuery).ConfigureAwait(false);
            return _InventoryTypeList;
        }

        internal async Task<List<KeyValuePairs>> GetInventoryStatusList()
            {
            /*
             * Fetch data from assetStatus
            */
            /*Your code goes here*/
            // "SELECT * FROM assetStatus";
            string myQuery = "SELECT id, name FROM assetstatus WHERE status = 1";
            List<KeyValuePairs> _InventoryStatusList = await Context.GetData<KeyValuePairs>(myQuery).ConfigureAwait(false);
            return _InventoryStatusList;

            }
        }
    }
