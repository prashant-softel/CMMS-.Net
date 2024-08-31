namespace CMMSAPIs.Models.DSM
{
    public class CMDSMData
    {
        public string fy { get; set; }
        public string month { get; set; }
        public string state { get; set; }
        public int stateId { get; set; }
        public string spv { get; set; }
        public int spv_id { get; set; }
        public string site { get; set; }
        public int site_id { get; set; }
        public string forcasterName { get; set; }
        public string category { get; set; }
        public int dsmPenalty { get; set; }
        public int scheduleKwh { get; set; }
        public int actualKwh { get; set; }
        public decimal dsmPer { get; set; }
        public int dsm_type_id { get; set; }
        public string dsmtype { get; set; }

    }

    public class CMDSMImportData
    {
        public string year { get; set; }
        public string fy { get; set; }
        public string month { get; set; }
        public string site { get; set; }
        public int site_id { get; set; }
        public string dsmType { get; set; }
        public string vendor { get; set; }
        public string category { get; set; }
        public int dsmPenalty { get; set; }
        public int scheduleKwh { get; set; }
        public int actualKwh { get; set; }
        public int dsm_type { get; internal set; }
        public int vendor_id { get; internal set; }
        public int category_id { get; internal set; }
        public dynamic dsm_panelty { get; internal set; }
        public dynamic schedule { get; internal set; }
        public dynamic actual { get; internal set; }
    }

    public class CMDSMFilter
    {
        public string[] fy { get; set; }
        public string[] month { get; set; }
        public int[] spvId { get; set; }
        public int[] stateId { get; set; }
        public int[] siteId { get; set; }
        public int[] DSMTypeId { get; set; }

    }

}
