using CMMSAPIs.Helper;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Utils;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

            internal Task<CMDefaultResponse> UpdateInventory(CMAddInventory request)
            {
                /*
                 * update all data in assets table and warranty table
                */
                /*Your code goes here*/
               // "SELECT* FROM assets JOIN assetwarranty ON assets.warrantyId = assetwarranty.id";
                return null;
            }

            internal Task<CMDefaultResponse> DeleteInventory(int id)
            {
                /*
                 * delete from assets and warranty table
                */
                /*Your code goes here*/



                return null;
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
