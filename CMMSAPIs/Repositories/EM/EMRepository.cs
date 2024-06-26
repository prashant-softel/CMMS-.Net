﻿using CMMSAPIs.Helper;
using CMMSAPIs.Models.EM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.EM
{
    public class EMRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public EMRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }
        public async Task<CMDefaultResponse> SetEscalationMatrix(List<CMSetMasterEM> request, int userID)
        {
            /*
             * Setting escalation matrix for each module and its every status
             * Master method where matrix is stored in EscalationMatrix table
             */
            List<int> idList = new List<int>();
            foreach (CMSetMasterEM emModule in request)
            {
                foreach (CMMasterEM emStatus in emModule.status_escalation)
                {
                    string deleteEscalation = $"DELETE FROM escalationmatrix WHERE moduleId = {emModule.module_id} AND statusId = {emStatus.status_id};";
                    await Context.ExecuteNonQry<int>(deleteEscalation).ConfigureAwait(false);
                    foreach (CMEscalation escalation in emStatus.escalation)
                    {
                        string addEscalation = "INSERT INTO escalationmatrix (moduleId, statusId, roleId, days, createdBy, createdAt) " +
                                                $"VALUES ({emModule.module_id}, {emStatus.status_id}, {escalation.role_id}, " +
                                                $"{escalation.days}, {userID}, '{UtilsRepository.GetUTCTime()}'); SELECT LAST_INSERT_ID(); ";
                        DataTable dt = await Context.FetchData(addEscalation).ConfigureAwait(false);
                        int id = Convert.ToInt32(dt.Rows[0][0]);
                        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.ESCALATION_MATRIX, id, 0, 0, "Escalation Details Added", CMMS.CMMS_Status.CREATED, userID);
                        idList.Add(id);
                    }
                }
            }
            return new CMDefaultResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, "Escalation Details Added");
        }
        //
        public async Task<List<GetEcMatrix>> GetEscalationMatrixList(CMMS.CMMS_Modules module)
        {
            /*
             * Gets the escalation matrix from EscalationMatrix table
             */

            string moduleCase = "CASE ";
            foreach (CMMS.CMMS_Modules mod in Enum.GetValues(typeof(CMMS.CMMS_Modules)))
            {
                moduleCase += $"WHEN moduleId = {(int)mod} THEN '{mod}' ";
            }
            moduleCase += "ELSE 'Invalid' END";

            string getModuleList = $"SELECT moduleId as module_id, {moduleCase} as module_name FROM escalationmatrix ";
            if (module > 0)
                getModuleList += $"WHERE moduleId = {(int)module} ";
            getModuleList += "GROUP BY moduleId;";

            List<GetEcMatrix> modules = await Context.GetData<GetEcMatrix>(getModuleList).ConfigureAwait(false);

            var response = new List<GetEcMatrix>();

            foreach (var mod in modules)
            {
                string statusCase = "CASE ";
                foreach (CMMS.CMMS_Status status in Enum.GetValues(typeof(CMMS.CMMS_Status)))
                {
                    statusCase += $"WHEN statusId = {(int)status} THEN '{status}' ";
                }
                statusCase += "ELSE 'Invalid' END";

                string getStatusList = $"SELECT statusId as status_Id, {statusCase} as status_name FROM escalationmatrix WHERE moduleId = {mod.module_id} GROUP BY statusId;";
                List<GetEcMatrix> statuses = await Context.GetData<GetEcMatrix>(getStatusList).ConfigureAwait(false);

                foreach (var status in statuses)
                {
                    string escalationQry = $"SELECT DISTINCT days, role.id as role_id, role.name as role_name FROM escalationmatrix " +
                                           $"LEFT JOIN userroles as role ON role.id = escalationmatrix.roleId " +
                                           $"WHERE statusId = {status.status_id} ORDER BY days;";
                    List<CMEscalation> escalations = await Context.GetData<CMEscalation>(escalationQry).ConfigureAwait(false);

                    response.Add(new GetEcMatrix
                    {
                        module_id = mod.module_id,
                        module_name = mod.module_name,
                        status_id = status.status_id,
                        status_name = status.status_name,
                        escalation = escalations
                    });
                }
            }

            return response;
        }

        public async Task<List<CMSetMasterEM>> GetEscalationMatrixbystatusId(CMMS.CMMS_Modules module, int status_id)
        {
            /*
             * Gets the escalation matrix from EscalationMatrix table
             */

            string moduleCase = "CASE ";
            foreach (CMMS.CMMS_Modules mod in Enum.GetValues(typeof(CMMS.CMMS_Modules)))
            {
                moduleCase += $"WHEN moduleId = {(int)mod} THEN '{mod}' ";
            }
            moduleCase += "ELSE 'Invalid' END";
            string getModuleList = $"SELECT moduleId as module_id, {moduleCase} as module_name FROM escalationmatrix ";
            if (module > 0)
                getModuleList += $"WHERE moduleId = {(int)module} ";
            getModuleList += "GROUP BY moduleId;";
            List<CMSetMasterEM> matrix = await Context.GetData<CMSetMasterEM>(getModuleList).ConfigureAwait(false);
            foreach (CMSetMasterEM mod in matrix)
            {
                string statusCase = "CASE ";
                foreach (CMMS.CMMS_Status status in Enum.GetValues(typeof(CMMS.CMMS_Status)))
                {
                    statusCase += $"WHEN statusId = {(int)status} THEN '{status}' ";
                }
                statusCase += "ELSE 'Invalid' END";
                string getStatusList = $"SELECT statusId as status_id, {statusCase} as status_name FROM escalationmatrix WHERE statusId = {status_id} GROUP BY statusId;";
                List<CMMasterEM> statusList = await Context.GetData<CMMasterEM>(getStatusList).ConfigureAwait(false);
                foreach (CMMasterEM status in statusList)
                {
                    string escalationQry = $"SELECT DISTINCT days, role.id as role_id, role.name as role_name FROM escalationmatrix " +
                                            $"LEFT JOIN userroles as role ON role.id = escalationmatrix.roleId " +
                                            $"WHERE statusId = {status.status_id} ORDER BY days;";
                    List<CMEscalation> escalation = await Context.GetData<CMEscalation>(escalationQry).ConfigureAwait(false);

                    status.escalation = escalation;
                }
                mod.status_escalation = statusList;
            }

            return matrix;
        }


        public async Task<CMEscalationResponse> Escalate(CMMS.CMMS_Modules module, int module_ref_id)
        {
            /*
             * Checks the current UTC time with the time since status was last updated
             * Performs escalation process as per escalation matrix
             * Will put escalation details in escalation table
             * Also sends notification to all users under the plant
             * 
             */
            CMEscalationResponse response = null;
            string qry0 = $"SELECT tableName, updateTimeColumn, statusColumn FROM moduletables WHERE softwareId = {(int)module};";
            DataTable dt0 = await Context.FetchData(qry0).ConfigureAwait(false);
            string qry1;
            if (dt0.Rows.Count > 0)
            {
                string table = Convert.ToString(dt0.Rows[0]["tableName"]);
                string timeCol = Convert.ToString(dt0.Rows[0]["updateTimeColumn"]);
                string statCol = Convert.ToString(dt0.Rows[0]["statusColumn"]);
                qry1 = $"SELECT {table}.{statCol} as status, {table}.{timeCol} as updateDate FROM {table} WHERE {table}.id = {module_ref_id};";
            }
            else
            {
                qry1 = $"SELECT status, CASE WHEN createdAt = '0000-00-00 00:00:00' THEN NULL ELSE createdAt END AS updateDate " +
                                $"FROM history WHERE (moduleType = {(int)module} OR secondaryModuleRefType = {(int)module}) " +
                                $"AND (moduleRefId = {module_ref_id} OR secondaryModuleRefId = {module_ref_id}) ORDER BY createdAt DESC;";
            }
            DataTable dt1 = await Context.FetchData(qry1).ConfigureAwait(false);
            int status = Convert.ToInt32(dt1.Rows[0]["status"]);
            string qry2 = $"SELECT days, roleId FROM escalationmatrix WHERE moduleId = {(int)module} AND statusId = {status} " +
                            $"ORDER BY days ASC;";
            DataTable dt2 = await Context.FetchData(qry2).ConfigureAwait(false);
            Dictionary<int, int> escalation = new Dictionary<int, int>();
            escalation.Merge(dt2.GetColumn<int>("days"), dt2.GetColumn<int>("roleId"));
            var diff = DateTime.UtcNow.Date - Convert.ToDateTime(dt1.Rows[0]["updateDate"]).Date;
            try
            {
                string qry3 = $"INSERT INTO escalationlog (moduleId, moduleRefId, moduleStatus, notifSentToId, notifSentAt) VALUES " +
                                $"({(int)module}, {module_ref_id}, {status}, {escalation[diff.Days]}, '{UtilsRepository.GetUTCTime()}'); " +
                                $"SELECT LAST_INSERT_ID(); ";
                DataTable dt3 = await Context.FetchData(qry3).ConfigureAwait(false);
                int newId = Convert.ToInt32(dt3.Rows[0][0]);
                string qry4 = $"SELECT id, loginId FROM users WHERE roleId >= {escalation[diff.Days]};";
                DataTable dt4 = await Context.FetchData(qry4).ConfigureAwait(false);
                List<int> userIds = dt4.GetColumn<int>("id");
                string qry5 = "INSERT INTO escalationsentto (escalationLogId, notifSentTo) VALUES ";
                foreach (int user in userIds)
                {

                    qry5 += $"({newId}, {user}), ";
                }
                qry5 = qry5.Substring(0, qry5.Length - 2);
                await Context.ExecuteNonQry<int>(qry5).ConfigureAwait(false);
                response = new CMEscalationResponse(module, module_ref_id, CMMS.RETRUNSTATUS.SUCCESS, "Escalation performed successfully");
            }
            catch (KeyNotFoundException)
            {

            }
            return response;
        }
        public async Task<List<CMEscalationLog>> ShowEscalationLog(CMMS.CMMS_Modules module, int module_ref_id)
        {
            /*
             * Returns the escalation log for the reference ID of module
             */
            string statusCase = "CASE ";
            foreach (CMMS.CMMS_Status status in Enum.GetValues(typeof(CMMS.CMMS_Status)))
            {
                statusCase += $"WHEN moduleStatus = {(int)status} THEN '{status}' ";
            }
            statusCase += "ELSE 'Invalid' END";
            string moduleCase = "CASE ";
            foreach (CMMS.CMMS_Modules modules in Enum.GetValues(typeof(CMMS.CMMS_Modules)))
            {
                moduleCase += $"WHEN moduleId = {(int)modules} THEN '{modules}' ";
            }
            moduleCase += "ELSE 'Invalid' END";
            string myQuery = $"SELECT moduleId as module_id, {moduleCase} as module_name, moduleStatus as status_id, " +
                                $"{statusCase} as status_name, moduleRefId as module_ref_id, notifSentAt as escalation_time, " +
                                $"role.id as escalated_to_role_id, role.name as escalated_to_role_name " +
                                $"FROM escalationlog as log LEFT JOIN userroles as role ON log.notifSentToId = role.id " +
                                $"WHERE moduleId = {(int)module} AND moduleRefId = {module_ref_id}";
            List<CMEscalationLog> log = await Context.GetData<CMEscalationLog>(myQuery).ConfigureAwait(false);
            return log;
        }
    }
}
