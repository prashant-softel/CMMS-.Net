using System;

namespace CMMSAPIs.Models.Masters
{
    public class MISSourceOfObservation
    {
        public int id { get; set; } 
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }

    public class MISTypeObservation
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }

    public class MISRiskType
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }

    public class MISCostType
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }

    public class MISGrievanceType
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }

    public class MISResolutionLevel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }
    public class BODYPARTS
    {
        public int sequence_no { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int Status { get; set; }
    }
    public class Responsibility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class CMIncidentType
    {
        public int id { get; set; }
        public string incidenttype { get; set; }
        public int status { get; set; }
       // public DateTime CreatedAt { get; set; }
       // public string CreatedBy { get; set; }
       // public DateTime UpdatedAt { get; set; }
        //public string UpdatedBy { get; set; }
        //changes
        public int addedBy { get; set; }
        public DateTime  addedAt { get; set; }
        public  int updatedBy { get; set; }
        public  DateTime updatedAt { get; set; }
    }

    public class CMMisWaterData
    {
        public int Id { get; set; } 
        public int PlantId { get; set; } 
        public DateTime Date { get; set; } 
        public int WaterTypeId { get; set; }
        public string Description { get; set; }
        public decimal DebitQty { get; set; } 
        public decimal CreditQty { get; set; } 
        public int AddedBy { get; set; } 
        public DateTime AddedAt { get; set; }
        public int UpdatedBy { get; set; } 
        public DateTime UpdatedAt { get; set; }
    }

    public class CMGetMisWaterData
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public DateTime Date { get; set; }
        public int WaterTypeId { get; set; }
        public string Description { get; set; }
        public decimal DebitQty { get; set; }
        public decimal CreditQty { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
