using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Facility;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Models.Notifications;

namespace CMMSAPIs.Repositories.Facility
{
    public class FacilityRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public FacilityRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

     
        internal async Task<List<CMFacilityList>> GetFacilityList()
        {
            string myQuery = "SELECT id, name, address, city, state, country, zipcode as pin FROM Facilities WHERE isBlock = 0";
            List<CMFacilityList> _Facility = await Context.GetData<CMFacilityList>(myQuery).ConfigureAwait(false);
            return _Facility;
        }

        internal async Task<List<CMFacilityList>> GetFacilityList(int facility_id)
        {
            string myQuery = "SELECT id, name, address, city, state, country, zipcode as pin FROM Facilities WHERE id= " + facility_id;
            List<CMFacilityList> _Facility = await Context.GetData<CMFacilityList>(myQuery).ConfigureAwait(false);
            return _Facility;
        }

        internal async Task<List<CMFacilityDetails>> GetFacilityDetails(int id)
        {
            string myQuery = "SELECT a.name, a.description, s.name AS status, f.name AS block_name, a2.name as parent_name, " +
                "b3.name as manufacturer_name, a.currency FROM assets AS a JOIN assetstatus as s on s.id = a.statusId " +
                "JOIN facilities as f ON f.id = a.blockId JOIN assets as a2 ON a.parentId = a2.id " +
                "JOIN business AS b2 ON a.ownerId = b2.id JOIN business AS b3 ON a.manufacturerId = b3.id";
            if (id != 0)
            {
                myQuery += " WHERE a.id= " + id;
            }
            List<CMFacilityDetails> _ViewInventoryList = await Context.GetData<CMFacilityDetails>(myQuery).ConfigureAwait(false);

            return _ViewInventoryList;


        }
        internal async Task<int> CreateNewFacility(CMCreateFacility request)
        { 
     
        string qryFacilityInsert = "insert into facilities " +
                                    "(name, description_del, customerId, ownerId, operatorId, isBlock, parentId, address, city, state, country, zipcode, latitude,longitude,createdBy,createdAt,status,photoId,description,timezone,startDate,endDate ) values" +
                                 $"('{ request.name }', '{ request.description_del }', { request.customerId }, { request.ownerId }, { request.operatorId }, { request.isBlock }, { request.parentId }, '{ request.address }', '{ request.city }', '{ request.state }', '{ request.country }', { request.zipcode }, { request.latitude }, { request.longitude }, { request.createdBy }, '{ UtilsRepository.GetUTCTime() }', { request.status }, { request.photoId }, '{ request.description }','{ request.timezone }', '{ UtilsRepository.GetUTCTime() }', '{ UtilsRepository.GetUTCTime() }' )";

            int insertedId = await Context.ExecuteNonQry<int>(qryFacilityInsert).ConfigureAwait(false);

            return insertedId;
        }

        internal async Task<int> UpdateFacility(CMUpdateFacility request)
        {
            string qryFacilityUpdate = $"update facilities set name = '{ request.name }', description_del='{ request.description_del }', customerId = { request.customerId }, ownerId={ request.ownerId }, operatorId={ request.operatorId }, isBlock={ request.isBlock }, parentId={ request.parentId }, address='{ request.address }', city='{ request.city }', state='{ request.state }', country='{ request.country }', zipcode={ request.zipcode }, latitude={ request.latitude }, longitude={ request.longitude }, updatedBy={ request.updatedBy }, updatedAt='{ UtilsRepository.GetUTCTime() }', status={ request.status }, photoId= { request.photoId }, description='{ request.description }', timezone='{ request.timezone }', startDate= '{ UtilsRepository.GetUTCTime() }', endDate='{ UtilsRepository.GetUTCTime() }' where id = { request.id } ;";

            int UpdatededId=await Context.ExecuteNonQry<int>(qryFacilityUpdate).ConfigureAwait(false);

            return UpdatededId;
        }

        internal async Task<CMDefaultResponse> DeleteFacility(int facility_id)
        {
            string DeleteQry = $"delete from facilities where id = {facility_id};";
            await Context.ExecuteNonQry<int>(DeleteQry).ConfigureAwait(false);

            CMDefaultResponse response = new CMDefaultResponse(facility_id, CMMS.RETRUNSTATUS.SUCCESS, "Facility Delete Successfull");
            return response;
        }
    }
}
