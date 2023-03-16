namespace CMMSAPIs.Models.Masters
{
    public class CMCreateCheckList
    {
        public int id { get; set; }
        public string checklist_number { get; set; }
        public int type { get; set; }
        public int facility_id { get; set; }
        public int category_id { get; set; }
        public int frequency_id { get; set; }
        public int manPower { get; set; }
        public int duration { get; set; }
        public int status { get; set; }
    }

    public class CMCheckList
    {
        public int id { get; set; }
        public string checklist_number { get; set; }
        public int type { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public int frequency_id { get; set; }
        public string frequency_name { get; set; }
        public int manPower { get; set; }
        public int duration { get; set; }
        public int status { get; set; }
        public int createdById { get; set; }
        public string createdByName { get; set; }
        public dynamic createdAt { get; set; }
        public int updatedById { get; set; }
        public string updatedByName { get; set; }
        public dynamic updatedAt { get; set; }
    }
}
