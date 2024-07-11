using CMMSAPIs.Helper;
using CMMSAPIs.Models.Facility;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Facility
{
    public class FacilityRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public FacilityRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }


        internal async Task<List<CMFacilityList>> GetFacilityList(int userID)
        {
            string myQuery = "SELECT facilities.id, facilities.name,spv.id as spv_id,spv.name as spv, facilities.address, facilities.city, facilities.state, facilities.country, facilities.zipcode as pin, Facilities.description FROM Facilities  inner join userfacilities uf on uf.facilityId = Facilities.id LEFT JOIN spv ON facilities.spvId=spv.id WHERE isBlock = 0 and facilities.status = 1 and uf.userId = " + userID + ";";
            List<CMFacilityList> _Facility = await Context.GetData<CMFacilityList>(myQuery).ConfigureAwait(false);
            return _Facility;
        }

        internal async Task<List<CMFacilityList>> GetBlockList(int parent_id)
        {
            if (parent_id <= 0)
                throw new ArgumentException("Invalid Parent ID");
            string myQuery = $"SELECT facilities.id, facilities.name, spv.name as spv, facilities.address, facilities.city, facilities.state, facilities.country, facilities.zipcode as pin FROM Facilities LEFT JOIN spv ON facilities.spvId=spv.id WHERE isBlock = 1 and parentId = {parent_id} and facilities.status = 1;";
            List<CMFacilityList> _Block = await Context.GetData<CMFacilityList>(myQuery).ConfigureAwait(false);
            return _Block;
        }

        internal async Task<CMFacilityDetails> GetFacilityDetails(int id, string facilitytimeZone)
        {
            string myQuery = $"SELECT facility.id as id, block.name as parentName, facility.name AS blockName, b.name as ownerName, spv.name as spvName, facility.customerId as customerId, facility.ownerId as ownerId, facility.operatorId as operatorId, facility.isBlock as isBlock, facility.parentId as parentId, facility.address as address, facility.city as city, facility.state as state, facility.country as country, facility.zipcode as zipcode, facility.latitude as latitude, facility.longitude as longitude, facility.createdBy as createdById, facility.createdAt as createdAt, facility.status as status, facility.photoId as photoId, facility.description as description, facility.timezone as timezone, facility.startDate as startDate, facility.endDate as endDate, CONCAT(u.firstName + ' ' + u.lastName) as createdByName FROM facilities as facility LEFT JOIN facilities as block on block.id = facility.parentId LEFT JOIN business AS b ON facility.ownerId = b.id LEFT JOIN spv ON facility.spvId=spv.id LEFT JOIN users as u ON u.id = facility.createdBy ";
            if (id > 0)
            {
                myQuery += " WHERE facility.id= " + id;
            }
            else
            {
                throw new ArgumentException("Invalid facility ID");
            }
            List<CMFacilityDetails> _GetFacilityDetails = await Context.GetData<CMFacilityDetails>(myQuery).ConfigureAwait(false);
            foreach (var detail in _GetFacilityDetails)
            {
                if (detail != null && detail.createdAt != null)
                    detail.createdAt = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.createdAt);
                if (detail != null && detail.endDate != null)
                    detail.endDate = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.endDate);
                if (detail != null && detail.startDate != null)
                    detail.startDate = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.startDate);

            }

            if (_GetFacilityDetails.Count == 0)
                throw new NullReferenceException($"Facility with ID {id} not found");
            return _GetFacilityDetails[0];
        }

        internal async Task<CMDefaultResponse> CreateNewFacility(CMCreateFacility request, int userID)
        {
            string country, state, city;
            string getCountryQry = $"SELECT name FROM countries WHERE id = {request.countryId};";
            DataTable dtCountry = await Context.FetchData(getCountryQry).ConfigureAwait(false);
            //if (dtCountry.Rows.Count == 0)
            //    throw new ArgumentException("Invalid Country");
            country = dtCountry.Rows.Count == 0 ? "" : Convert.ToString(dtCountry.Rows[0][0]);

            string getStateQry = $"SELECT name FROM states WHERE id = {request.stateId};";
            DataTable dtState = await Context.FetchData(getStateQry).ConfigureAwait(false);
            //if (dtState.Rows.Count == 0)
            //    throw new ArgumentException("Invalid State");
            state = dtState.Rows.Count == 0 ? "" : Convert.ToString(dtState.Rows[0][0]);

            string getCityQry = $"SELECT name FROM cities WHERE id = {request.cityId};";
            DataTable dtCity = await Context.FetchData(getCityQry).ConfigureAwait(false);
            //if (dtCity.Rows.Count == 0)
            //    throw new ArgumentException("Invalid City");
            city = dtCity.Rows.Count == 0 ? "" : Convert.ToString(dtCity.Rows[0][0]);

            //string myQuery1 = $"SELECT * FROM states WHERE id = {request.stateId} AND country_id = {request.countryId};";
            //DataTable dt1 = await Context.FetchData(myQuery1).ConfigureAwait(false);
            ////if (dt1.Rows.Count == 0)
            ////    throw new ArgumentException($"{state} is not situated in {country}");
            //string myQuery2 = $"SELECT * FROM cities WHERE id = {request.cityId} AND state_id = {request.stateId} AND country_id = {request.countryId};";
            //DataTable dt2 = await Context.FetchData(myQuery2).ConfigureAwait(false);
            ////if (dt2.Rows.Count == 0)
            ////    throw new ArgumentException($"{city} is not situated in {state}, {country}");

            string qryFacilityInsert = "insert into facilities(name, spvId, customerId, ownerId, operatorId, isBlock, parentId, " +
                                        "address, country, state, city, zipcode, countryId, stateId, cityId, latitude, longitude, " +
                                        "createdBy, createdAt, status, photoId, description, timezone, startDate, endDate) " +
                                        $"values ('{request.name}', {request.spvId}, {request.customerId}, {request.ownerId}, {request.operatorId}, " +
                                        $"0, 0, '{request.address}', '{country}', '{state}', '{city}', {request.zipcode}, {request.countryId}, " +
                                        $"{request.stateId}, {request.cityId}, {request.latitude}, {request.longitude}, {userID}, " +
                                        $"'{UtilsRepository.GetUTCTime()}', 1, {request.photoId}, '{request.description}', '{request.timezone}', " +
                                        $"'{(request.startDate != null ? ((DateTime)request.startDate).ToString("yyyy-MM-dd HH:mm:ss") : UtilsRepository.GetUTCTime())}', " +
                                        $"'{(request.endDate != null ? ((DateTime)request.endDate).ToString("yyyy-MM-dd HH:mm:ss") : UtilsRepository.GetUTCTime())}'); " +
                                        $"SELECT LAST_INSERT_ID();";

            DataTable dt = await Context.FetchData(qryFacilityInsert).ConfigureAwait(false);
            int facility_id = Convert.ToInt32(dt.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(facility_id, CMMS.RETRUNSTATUS.SUCCESS, "Facility created successfully");
            return response;
        }



        internal async Task<CMDefaultResponse> CreateNewBlock(CMCreateBlock request, int userID)
        {
            string myQuery = $"INSERT INTO facilities(name, description, isBlock, parentId, photoId, status, createdBy, createdAt) " +
                                $"VALUES ('{request.name}', '{request.description}', 1, {request.parentId}, {request.photoId}, 1, " +
                                $"{userID}, '{UtilsRepository.GetUTCTime()}'); SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            string updateQry = $"UPDATE facilities as blocks JOIN facilities as plants ON blocks.parentId = plants.id SET " +
                                $"blocks.customerId = plants.customerId,blocks.spvId = plants.spvId, blocks.ownerId = plants.ownerId, blocks.operatorId = plants.operatorId, " +
                                $"blocks.countryId = plants.countryId, blocks.stateId = plants.stateId, blocks.cityId = plants.cityId, blocks.address = plants.name, " +
                                $"blocks.country = plants.country, blocks.state = plants.state, blocks.city = plants.city, blocks.timezone = plants.timezone, " +
                                $"blocks.zipcode = plants.zipcode, blocks.latitude = plants.latitude, blocks.longitude = plants.longitude WHERE blocks.id = {id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Block Created Successfully");
        }
        internal async Task<CMDefaultResponse> UpdateBlock(CMCreateBlock request, int userID)
        {
            string updateQry = "UPDATE facilities SET ";
            string updateParentQry = "";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            if (request.photoId > 0)
                updateQry += $"photoId = {request.photoId}, ";
            if (request.parentId > 0)
            {
                updateQry += $"parentId = {request.parentId}, ";
                updateParentQry = $"UPDATE facilities as blocks JOIN facilities as plants ON blocks.parentId = plants.id SET " +
                                    $"blocks.customerId = plants.customerId, blocks.ownerId = plants.ownerId, blocks.operatorId = plants.operatorId, " +
                                    $"blocks.countryId = plants.countryId, blocks.stateId = plants.stateId, blocks.cityId = plants.cityId, blocks.address = plants.name, " +
                                    $"blocks.country = plants.country, blocks.state = plants.state, blocks.city = plants.city, blocks.timezone = plants.timezone, " +
                                    $"blocks.zipcode = plants.zipcode, blocks.latitude = plants.latitude, blocks.longitude = plants.longitude WHERE blocks.id = {request.id};";
            }
            updateQry += $"updatedBy = {userID}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            if (updateParentQry != null && updateParentQry != "")
                await Context.ExecuteNonQry<int>(updateParentQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Block Details updated successfully");
        }
        internal async Task<CMDefaultResponse> UpdateFacility(CMCreateFacility request, int userID)
        {
            string locationQry = $"SELECT countryId, stateId, cityId FROM facilities WHERE id = {request.id};";
            DataTable dt0 = await Context.FetchData(locationQry).ConfigureAwait(false);
            string country, state, city;
            DataTable dtCountry, dtState, dtCity;
            string getCountryQry = $"SELECT name FROM countries WHERE id = {request.countryId};";
            dtCountry = await Context.FetchData(getCountryQry).ConfigureAwait(false);
            if (dtCountry.Rows.Count == 0)
            {
                request.countryId = Convert.ToInt32(dt0.Rows[0]["countryId"]);
                getCountryQry = $"SELECT name FROM countries WHERE id = {request.countryId};";
                dtCountry = await Context.FetchData(getCountryQry).ConfigureAwait(false);
            }
            country = Convert.ToString(dtCountry.Rows[0][0]);
            string getStateQry = $"SELECT name FROM states WHERE id = {request.stateId};";
            dtState = await Context.FetchData(getStateQry).ConfigureAwait(false);
            if (dtState.Rows.Count == 0)
            {
                request.stateId = Convert.ToInt32(dt0.Rows[0]["stateId"]);
                getStateQry = $"SELECT name FROM states WHERE id = {request.stateId};";
                dtState = await Context.FetchData(getStateQry).ConfigureAwait(false);
            }
            state = Convert.ToString(dtState.Rows[0][0]);
            string getCityQry = $"SELECT name FROM cities WHERE id = {request.cityId};";
            dtCity = await Context.FetchData(getCityQry).ConfigureAwait(false);
            if (dtCity.Rows.Count == 0)
            {
                request.cityId = Convert.ToInt32(dt0.Rows[0]["cityId"]);
                getCityQry = $"SELECT name FROM cities WHERE id = {request.cityId};";
                dtCity = await Context.FetchData(getCityQry).ConfigureAwait(false);
            }
            city = Convert.ToString(dtCity.Rows[0][0]);
            string myQuery1 = $"SELECT * FROM states WHERE id = {request.stateId} AND country_id = {request.countryId};";
            DataTable dt1 = await Context.FetchData(myQuery1).ConfigureAwait(false);
            if (dt1.Rows.Count == 0)
                throw new ArgumentException($"{state} is not situated in {country}");
            string myQuery2 = $"SELECT * FROM cities WHERE id = {request.cityId} AND state_id = {request.stateId} AND country_id = {request.countryId};";
            DataTable dt2 = await Context.FetchData(myQuery2).ConfigureAwait(false);
            if (dt2.Rows.Count == 0)
                throw new ArgumentException($"{city} is not situated in {state}, {country}");
            string qryFacilityUpdate = "UPDATE facilities SET ";
            if (request.name != "" && request.name != null)
                qryFacilityUpdate += $"name = '{request.name}', ";
            if (request.spvId > 0)
                qryFacilityUpdate += $"spvId = {request.spvId}, ";
            if (request.customerId > 0)
                qryFacilityUpdate += $"customerId = {request.customerId}, ";
            if (request.ownerId > 0)
                qryFacilityUpdate += $"ownerId = {request.ownerId}, ";
            if (request.operatorId > 0)
                qryFacilityUpdate += $"operatorId = {request.operatorId}, ";
            if (request.zipcode > 0)
                qryFacilityUpdate += $"zipcode = {request.zipcode}, ";
            if (request.photoId > 0)
                qryFacilityUpdate += $"photoId = {request.photoId}, ";
            if (country != null && country != "")
                qryFacilityUpdate += $"countryId = {request.countryId}, country = '{country}', ";
            if (state != null && state != "")
                qryFacilityUpdate += $"stateId = {request.stateId}, state = '{state}', ";
            if (city != null && city != "")
                qryFacilityUpdate += $"cityId = {request.cityId}, city = '{city}', ";
            if (request.address != null && request.address != "")
                qryFacilityUpdate += $"address = '{request.address}', ";
            if (request.description != null && request.description != "")
                qryFacilityUpdate += $"description = '{request.description}', ";
            if (request.timezone != null && request.timezone != "")
                qryFacilityUpdate += $"timezone = '{request.timezone}', ";
            if (request.startDate != null)
                qryFacilityUpdate += $"startDate = '{((DateTime)request.startDate).ToString("yyyy-MM-dd HH:mm:ss")}', ";
            if (request.endDate != null)
                qryFacilityUpdate += $"endDate = '{((DateTime)request.endDate).ToString("yyyy-MM-dd HH:mm:ss")}', ";
            qryFacilityUpdate += $"updatedBy = {userID}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(qryFacilityUpdate).ConfigureAwait(false);

            string updateChildQry = $"UPDATE facilities as blocks JOIN facilities as plants ON blocks.parentId = plants.id SET " +
                                    $"blocks.customerId = plants.customerId, blocks.ownerId = plants.ownerId, blocks.operatorId = plants.operatorId, " +
                                    $"blocks.countryId = plants.countryId, blocks.stateId = plants.stateId, blocks.cityId = plants.cityId, blocks.address = plants.name, " +
                                    $"blocks.country = plants.country, blocks.state = plants.state, blocks.city = plants.city, blocks.timezone = plants.timezone, " +
                                    $"blocks.zipcode = plants.zipcode, blocks.latitude = plants.latitude, blocks.longitude = plants.longitude WHERE plants.id = {request.id};";
            await Context.ExecuteNonQry<int>(updateChildQry).ConfigureAwait(false);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Facility Details Updated Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> DeleteFacility(int facility_id)
        {
            string DeleteQry = $"update facilities set status = 0 where id = {facility_id} or parentId = {facility_id};";
            await Context.ExecuteNonQry<int>(DeleteQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(facility_id, CMMS.RETRUNSTATUS.SUCCESS, "Facility Deleted Successfully");
            return response;
        }
        internal async Task<CMDefaultResponse> DeleteBlock(int block_id)
        {
            string DeleteQry = $"update facilities set status = 0 where id = {block_id};";
            await Context.ExecuteNonQry<int>(DeleteQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(block_id, CMMS.RETRUNSTATUS.SUCCESS, "Block Deleted Successfully");
            return response;
        }
        internal async Task<List<FacilityListEmployee>> GetFacilityListEmployee(int facility_id)
        {
            string myQuery = $"SELECT u.id, loginId as login_id, concat(firstName,' ', lastName) as name,uf.isemployee, birthday as birthdate, gender, mobileNumber, cities.name as city, states.name as state, countries.name as country, zipcode as pin,ud.designationName as designation FROM Users as u JOIN UserFacilities as uf ON u.id = uf.userId LEFT JOIN cities as cities ON cities.id = u.cityId LEFT JOIN states as states ON states.id = u.stateId and states.id = cities.state_id LEFT JOIN countries as countries ON countries.id = u.countryId and countries.id = cities.country_id and countries.id = states.country_id LEFT JOIN userdesignation as ud ON ud.id =u.designation_id LEFT JOIN usersaccess as access ON u.id = access.userId WHERE uf.isemployee=1 and  u.status = 1 AND uf.status = 1  AND u.isEmployee = 1 AND uf.facilityId = {facility_id}  GROUP BY u.id ORDER BY u.id ;";
            List<FacilityListEmployee> _Facility = await Context.GetData<FacilityListEmployee>(myQuery).ConfigureAwait(false);


            foreach (FacilityListEmployee emp in _Facility)
            {

                emp.responsibilityIds = new int[2];
                emp.responsibilityIds[0] = 1;
                emp.responsibilityIds[1] = 2;

            }


            return _Facility;
        }
        internal async Task<List<FacilityListEmployee>> GetEmployeeListbyFeatureId(int facility_id, int featureid, int isattendence)
        {
            List<FacilityListEmployee> _FacilityByFeatureid = new List<FacilityListEmployee>();

            if (isattendence == 0)
            {
                string myQuery = $"SELECT u.id, loginId as login_id, concat(firstName,' ', lastName) as name,uf.isemployee, usd.designationName as designation, birthday as birthdate, gender, mobileNumber, cities.name as city, states.name as state, countries.name as country, zipcode as pin  " +
                                $"FROM  Users as u  JOIN UserFacilities as uf ON u.id = uf.userId  " +
                                $"LEFT JOIN cities as cities ON cities.id = u.cityId  " +
                                $"LEFT JOIN states as states ON states.id = u.stateId and states.id = cities.state_id  " +
                                $"LEFT JOIN countries as countries ON countries.id = u.countryId and countries.id = cities.country_id and countries.id = states.country_id  " +
                                $"LEFT JOIN usersaccess as access ON u.id = access.userId  " +
                                $"LEFT JOIN  userdesignation as usd on  usd.id = u.designation_id " +
                                $"WHERE uf.isemployee = 1 and access.featureId ={featureid} and access.edit = 1 and u.status = 1 " +
                                $" AND  uf.status = 1  " +
                                $" AND uf.facilityId = {facility_id} GROUP BY u.id ORDER BY u.id;";
                _FacilityByFeatureid = await Context.GetData<FacilityListEmployee>(myQuery).ConfigureAwait(false);
            }
            else
            {
                string myQuery = $"SELECT u.id, loginId as login_id, concat(firstName,' ', lastName) as name,uf.isemployee, usd.designationName as designation, birthday as birthdate, gender, mobileNumber, cities.name as city, states.name as state, countries.name as country, zipcode as pin  " +
                                 $"FROM  Users as u  JOIN UserFacilities as uf ON u.id = uf.userId  " +
                                 $"LEFT JOIN cities as cities ON cities.id = u.cityId  " +
                                 $"LEFT JOIN states as states ON states.id = u.stateId and states.id = cities.state_id  " +
                                 $"LEFT JOIN countries as countries ON countries.id = u.countryId and countries.id = cities.country_id and countries.id = states.country_id  " +
                                 $"LEFT JOIN usersaccess as access ON u.id = access.userId  " +
                                 $"LEFT JOIN  userdesignation as usd on  usd.id = u.designation_id " +
                                 $"LEFT JOIN employee_attendance as es on es.employee_id = u.id " +
                                 $"WHERE uf.isemployee = 1 and access.featureId ={featureid} and access.edit = 1 and u.status = 1 " +
                                 $"AND  es.present=1 and es.Date=current_date()   AND es.in_time<now() AND (es.out_time<now() or es.out_time is null)  AND uf.status = 1  " +
                                 $"AND uf.facilityId = {facility_id} GROUP BY u.id ORDER BY u.id;";
                _FacilityByFeatureid = await Context.GetData<FacilityListEmployee>(myQuery).ConfigureAwait(false);
            }
            foreach (FacilityListEmployee emp in _FacilityByFeatureid)
            {

                emp.responsibilityIds = new int[2];
                emp.responsibilityIds[0] = 1;
                emp.responsibilityIds[1] = 2;
            }
            return _FacilityByFeatureid;
        }

    }
}

