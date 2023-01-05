namespace CMMSAPIs.Models.Utils
{
    public class ApprovalModel
    {
        public int id { get; set; }
        public string commnet { get; set; }
        public int status { get; set; }
        public int employee_id { get; set; }
        public dynamic Date { get; set; }
        public int Time { get; set; }
    }
}
