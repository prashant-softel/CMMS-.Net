using CMMSAPIs.Helper;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Inventory
{
    public class InventoryRepository : GenericRepository
    {
        public InventoryRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal Task<List<CMInventoryList>> GetInventoryList(int facility_id)
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
            return null;
        }

        internal Task<CMViewInventory> ViewInventory(int id)
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
            return null;
        }

        internal Task<CMDefaultResponse> AddInventory(CMAddInventory request)
        {
            /*
             * Add all data in assets table and warranty table
            */
            /*Your code goes here*/
            return null;
        }

        internal Task<CMDefaultResponse> UpdateInventory(CMAddInventory request)
        {
            /*
             * update all data in assets table and warranty table
            */
            /*Your code goes here*/
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

        internal Task<List<KeyValuePairs>> GetInventoryTypeList()
        {
            /*
             * Fetch data from assetType
            */
            /*Your code goes here*/
            return null;
        }

        internal Task<List<KeyValuePairs>> GetInventoryStatusList()
        {
            /*
             * Fetch data from assetStatus
            */
            /*Your code goes here*/
            return null;
        }
    }
}
