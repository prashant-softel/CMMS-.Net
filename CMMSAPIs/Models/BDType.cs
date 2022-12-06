namespace CMMSAPIs.Models
{
    enum BD_TYPE
    {
        eUSMH = 1,
        eSMH,
        eIGBD,
        eEGBD,
        eLoadShedding,
        eOthersHour,
        eLULL
    }
    public class BDType
    {
        public int bd_type_id { get; set; }
        public string bd_type_name { get; set; }
        public string description { get; set; }
        public string abbr { get; set; }
    }
}
