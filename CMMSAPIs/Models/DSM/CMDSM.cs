namespace CMMSAPIs.Models.DSM
{
    public class CMDSMData
    {
        public string fy { get; set; }
        public string month { get; set; }
        public string state { get; set; }
        public string spv { get; set; }
        public string site { get; set; }
        public string dsmType { get; set; }
        public string forcasterName { get; set; }
        public string category { get; set; }
        public int dsmPenalty { get; set; }
        public int scheduleKwh { get; set; }
        public int actualKwh { get; set; }
        public decimal dsmPer { get; set; }
        public int dsm_type_id { get; set; }
        public string dsmtype { get; set; }

    }
    /* {"FY",new Tuple<string, Type>("year",typeof(string))},
    { "Month",new Tuple<string, Type>("month", typeof(string))},
    { "Site",new Tuple<string, Type>("site_id", typeof(int))},
    { "DSM Type",new Tuple<string, Type>("dsm_type", typeof(int))},
    { "Vendor",new Tuple<string, Type>("vendor_id", typeof(int))},
    { "Category",new Tuple<string, Type>("category_id", typeof(int))},
    { "DSM Penalty (Rs.)",new Tuple<string, Type>("dsm_panelty", typeof(Int64))},
    { "Schedule (kWh)",new Tuple<string, Type>("schedule", typeof(Int64))},
    { "Actual (kWh)",new Tuple<string, Type>("actual", typeof(Int64))},*/
    public class CMDSMImportData
    {

        public int year { get; set; }
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
    public class DSMTYPE
    {
        public int id { get; set; }
        public string name { get; set; }

    }

}
