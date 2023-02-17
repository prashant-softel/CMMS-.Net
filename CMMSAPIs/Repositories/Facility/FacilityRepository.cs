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
            string myQuery = "SELECT facility.id as id, asset.name as parentName, facility.name AS blockName, b.name as ownerName, facility.customerId as customerId, facility.ownerId as ownerId, facility.operatorId as operatorId, facility.isBlock as isBlock, facility.parentId as parentId, facility.address as address, facility.city as city, facility.state as state, facility.country as country, facility.zipcode as zipcode, facility.latitude as latitude, facility.longitude as longitude, facility.createdBy as createdById, facility.createdAt as createdAt, facility.status as status, facility.photoId as photoId, facility.description as description, facility.timezone as timezone, facility.startDate as startDate, facility.endDate as endDate, CONCAT(u.firstName + ' ' + u.lastName) as createdByName  FROM softel_cmms.facilities as facility JOIN assets as asset on asset.parentId = facility.parentId JOIN business AS b ON facility.ownerId = b.id LEFT JOIN users as u ON u.id = facility.createdBy ";
            if (id != 0)
            {
                myQuery += " WHERE facility.id= " + id;
            }
            List<CMFacilityDetails> _GetFacilityDetails = await Context.GetData<CMFacilityDetails>(myQuery).ConfigureAwait(false);

            return _GetFacilityDetails;


        }
        internal async Task<int> CreateNewFacility(CMCreateFacility request)
        { 
     
        string qryFacilityInsert = "insert into facilities " +
                                    "(name, customerId, ownerId, operatorId, isBlock, parentId, address, city, state, country, zipcode, latitude,longitude,createdBy,createdAt,status,photoId,description,timezone,startDate,endDate ) values" +
                                 $"('{ request.name }', { request.customerId }, { request.ownerId }, { request.operatorId }, { request.isBlock }, { request.parentId }, '{ request.address }', '{ request.city }', '{ request.state }', '{ request.country }', { request.zipcode }, { request.latitude }, { request.longitude }, { request.createdBy }, '{ UtilsRepository.GetUTCTime() }', { request.status }, { request.photoId }, '{ request.description }','{ request.timezone }', '{ UtilsRepository.GetUTCTime() }', '{ UtilsRepository.GetUTCTime() }' )";

            int insertedId = await Context.ExecuteNonQry<int>(qryFacilityInsert).ConfigureAwait(false);

            return insertedId;
        }

        internal async Task<int> UpdateFacility(CMUpdateFacility request)
        {
            string qryFacilityUpdate = $"update facilities set name = '{ request.name }', customerId = { request.customerId }, ownerId={ request.ownerId }, operatorId={ request.operatorId }, isBlock={ request.isBlock }, parentId={ request.parentId }, address='{ request.address }', city='{ request.city }', state='{ request.state }', country='{ request.country }', zipcode={ request.zipcode }, latitude={ request.latitude }, longitude={ request.longitude }, updatedBy={ request.updatedBy }, updatedAt='{ UtilsRepository.GetUTCTime() }', status={ request.status }, photoId= { request.photoId }, description='{ request.description }', timezone='{ request.timezone }', startDate= '{ UtilsRepository.GetUTCTime() }', endDate='{ UtilsRepository.GetUTCTime() }' where id = { request.id } ;";

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
