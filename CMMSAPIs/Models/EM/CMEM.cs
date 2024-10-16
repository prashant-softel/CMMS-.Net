﻿using CMMSAPIs.Helper;
using System.Collections.Generic;

namespace CMMSAPIs.Models.EM
{
       
    public class CMNotification
    {
        public int facilityId { get; set; }
        public CMMS.CMMS_Modules moduleId { get; set; }
        public CMMS.CMMS_Status statusId { get; set; }
        public int moduleRefId { get; set; }        
        public string message { get; set; }                 //Optional
        public string additionalUserIds { get; set; }    //Optional. comma delimited string
    }

    public class CMSetMasterEM
    {
        public int module_id { get; set; }
        public string module_name { get; set; }
        public List<CMMasterEM> status_escalation { get; set; }
    }
    public class CMMasterEM
    {
        public int status_id { get; set; }
        public string status_name { get; set; }
        public List<CMEscalation> escalation { get; set; }
    }
    public class CMEscalation
    {
        public int days { get; set; }
        public int role_id { get; set; }
        public string role_name { get; set; }
    }
    public class CMEscalationLog
    {
        public int module_id { get; set; }
        public string module_name { get; set; }
        public int module_ref_id { get; set; }
        public dynamic escalation_time { get; set; }
        public int escalated_to_role_id { get; set; }
        public string escalated_to_role_name { get; set; }
        public int status_id { get; set; }
        public string status_name { get; set; }
    }
    public class GetEcMatrix
    {
        public int module_id { get; set; }
        public string module_name { get; set; }
        public int status_id { get; set; }
        public string status_name { get; set; }
        public List<CMEscalation> escalation { get; set; }
    }
}
